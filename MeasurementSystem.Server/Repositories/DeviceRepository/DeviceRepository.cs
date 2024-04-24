using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Exceptions;
using InfluxDB.Client.Writes;
using MeasurementSystemWebAPI.Contexts;
using MeasurementSystemWebAPI.Models;
using MeasurementSystemWebAPI.Repositories.DeviceInfoRepository;
using Newtonsoft.Json;
using System.Dynamic;
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
            var query = $"from(bucket: \"{dbContext.Bucket}\") |> range(start: {from.Subtract(DateTime.UnixEpoch).TotalSeconds}, " +
                $"stop: {to.Subtract(DateTime.UnixEpoch).TotalSeconds})";
            var tables = await dbContext.InfluxDBClient.GetQueryApi().QueryAsync(query, dbContext.Org);

            var data = new Dictionary<string, Dictionary<DateTime, IDictionary<string, object>>>();

            foreach (var table in tables)
            {
                foreach (var record in table.Records)
                {
                    var authKey = record.Values["_measurement"].ToString();
                    var utcTime = record.Values["_time"];
                    var localTime = DateTime.Parse(utcTime.ToString());
                    var field = record.Values["_field"].ToString();
                    var value = record.Values["_value"];

                    data.TryAdd(authKey, []);
                    data[authKey].TryAdd(localTime, new ExpandoObject());
                    data[authKey][localTime].Add(field, value);
                }
            }

            var records = new Dictionary<string, Record>();
            var index = 0;
            var deviceInfos = deviceInfoRepository.Select();

            foreach (var (authKey, dateToFields) in data)
            {
                var deviceInfo = deviceInfos.FirstOrDefault(d => d.AuthKey == authKey)
                    ?? throw new NotFoundException($"Невалидный ключ аутентификации устройства: {authKey}");

                foreach (var (date, fields) in dateToFields)
                {
                    var record = new Record()
                    {
                        Date = date.ToString("yyyy-MM-dd HH:mm:ss"),
                        DeviceName = deviceInfo.Name,
                        DeviceSerial = deviceInfo.Serial,
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

            string result = JsonConvert.SerializeObject(keyValuePairs);
            Console.WriteLine(result);
        }
    }
}
