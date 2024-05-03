using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Exceptions;
using InfluxDB.Client.Writes;
using MeasurementSystemWebAPI.Contexts;
using MeasurementSystemWebAPI.Models;
using MeasurementSystemWebAPI.Repositories.DeviceInfoRepository;
using Newtonsoft.Json;
using System.Text;

namespace MeasurementSystemWebAPI.Repositories.DeviceRepository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IDeviceInfoRepository deviceInfoRepository;
        private readonly DeviceContext dbContext;

        public DeviceRepository(IDeviceInfoRepository deviceInfoRepository)
        {
            this.deviceInfoRepository = deviceInfoRepository;
            dbContext = new DeviceContext();
        }     

        public async Task<Dictionary<string, Record>> SelectAsync(DateTime from, DateTime to)
        {
            var query = $"from(bucket: \"{dbContext.Bucket}\") |> range(start: {from.AddHours(-3).Subtract(DateTime.UnixEpoch).TotalSeconds}, " +
                $"stop: {to.AddHours(-3).Subtract(DateTime.UnixEpoch).TotalSeconds}) " +
                $"|> pivot(rowKey:[\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")";
            var tables = await dbContext.InfluxDBClient.GetQueryApi().QueryAsync(query, dbContext.Org);

            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));
            var records = new Dictionary<string, Record>();
            var index = 0;

            foreach (var table in tables)
            {
                foreach (var tr in table.Records)
                {
                    var authKey = tr.Values["_measurement"].ToString();
                    var utcTime = tr.Values["_time"];
                    var localTime = DateTime.Parse(utcTime.ToString()).AddHours(3).ToString("yyyy-MM-dd HH:mm:ss");
                    var fields = tr.Values.Skip(6).ToDictionary(r => r.Key, r => r.Value);

                    var record = new Record()
                    {
                        Date = localTime,
                        DeviceName = deviceInfo[authKey].Name,
                        DeviceSerial = deviceInfo[authKey].Serial,
                        Data = fields
                    };
                    records.Add(index.ToString(), record);
                    ++index;
                }
            }

            return records;
        }

        public async Task<byte[]> SelectAsBytesAsync(DateTime from, DateTime to)
        {
            var records = await SelectAsync(from, to);
            var fileContents = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(records));
            return fileContents;
        }

        public void Insert(string device)
        {
            JsonTextReader reader = new(new StringReader(device));
            Dictionary<string, string> keyValuePairs = new();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    string nodeName = reader.Value.ToString();
                    reader.Read();
                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                    {
                        string propertyName = reader.Value.ToString();
                        reader.Read();

                        if (reader.Value != null && reader.TokenType != JsonToken.StartObject)
                        {
                            keyValuePairs.Add($"{nodeName}_{propertyName}", reader.Value.ToString());
                        }
                        else
                        {
                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                            {
                                string additionalProperty = reader.Value.ToString();
                                reader.Read();

                                if (reader.Value != null && reader.TokenType != JsonToken.StartObject)
                                {
                                    keyValuePairs.Add($"{nodeName}_{propertyName}_{additionalProperty}", reader.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }

            if (!keyValuePairs.ContainsKey("system_Akey"))
            {
                throw new Exception("Нет ключа аутентификации");
            }

            var authKey = keyValuePairs["system_Akey"];
            keyValuePairs.Remove("system_Akey");

            var deviceInfo = deviceInfoRepository.Select().FirstOrDefault(d => d.AuthKey == authKey) 
                ?? throw new NotFoundException($"Невалидный ключ аутентификации устройства: {authKey}");

            var point = PointData.Measurement(authKey);

            foreach (var pair in keyValuePairs)
            {
                point = double.TryParse(pair.Value, out double value) ? point.Field(pair.Key, value)
                    : point = point.Field(pair.Key, pair.Value);
            }

            point = point.Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using (var writeApi = dbContext.InfluxDBClient.GetWriteApi())
            {
                writeApi.WritePoint(point, dbContext.Bucket, dbContext.Org);
            }
        }
    }
}
