using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Exceptions;
using InfluxDB.Client.Writes;
using MeasurementSystem.Server.Contexts;
using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Repositories.DeviceInfoRepository;
using Newtonsoft.Json;
using System.Text;

namespace MeasurementSystem.Server.Repositories.DeviceRepository
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

        public async Task<Dictionary<string, IEnumerable<string>>> SelectDeviceFieldsAsync()
        {
            /*var query = $"from(bucket: \"{dbContext.Bucket}\") |> range(start: -7d) " +
                $"|> pivot(rowKey:[\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")" +
                $"|> limit(n: 1)";
            var tables = await dbContext.InfluxDBClient.GetQueryApi().QueryAsync(query, dbContext.Org);

            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));
            var result = new Dictionary<string, IEnumerable<string>>();

            foreach (var table in tables)
            {
                foreach (var tr in table.Records)
                {
                    var authKey = tr.Values["_measurement"].ToString();
                    var fields = tr.Values.Keys.Skip(6).Where(f => !f.Contains("system"));
                    var key = $"{deviceInfo[authKey].Name}({deviceInfo[authKey].Serial})";
                    result.Add(key, fields);
                }
            }

            return result;*/

            var deviceInfos = deviceInfoRepository.Select();
            var result = new Dictionary<string, IEnumerable<string>>();

            foreach (var info in deviceInfos)
            {
                var query = "import \"influxdata/influxdb/schema\"" +
                    $"schema.measurementFieldKeys(bucket: \"{dbContext.Bucket}\", measurement: \"{info.AuthKey}\")";

                var tables = await dbContext.InfluxDBClient.GetQueryApi().QueryAsync(query, dbContext.Org);
                var fields = new List<string>();
                foreach (var tr in tables.First().Records)
                {
                    var field = tr.Values["_value"].ToString();

                    if (!field.Contains("system"))
                    {
                        fields.Add(field);
                    }
                }
                result.Add($"{info.Name}({info.Serial})", fields);
            }

            return result;
        }

        public Device Insert(string device)
        {
            JsonTextReader reader = new(new StringReader(device));
            var keyValuePairs = new Dictionary<string, string>();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    string nodeName = reader.Value.ToString();
                    reader.Read();
                    while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                    {
                        string? propertyName = (reader?.Value?.ToString()) 
                            ?? throw new InvalidOperationException("Свойство не может быть null");
                        reader.Read();

                        if (reader.Value != null && reader.TokenType != JsonToken.StartObject)
                        {
                            var value = (reader.Value.ToString()) 
                                ?? throw new InvalidOperationException($"Значение свойства {propertyName} не может быть null");
                            keyValuePairs.Add($"{nodeName}_{propertyName}", value);
                        }
                        else
                        {
                            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                            {
                                string additionalProperty = (reader?.Value?.ToString()) 
                                    ?? throw new InvalidOperationException("Свойство не может быть null");
                                reader.Read();

                                if (reader.Value != null && reader.TokenType != JsonToken.StartObject)
                                {
                                    var value = (reader.Value.ToString()) 
                                        ?? throw new InvalidOperationException($"Значение свойства {propertyName} не может быть null");
                                    keyValuePairs.Add($"{nodeName}_{propertyName}_{additionalProperty}", value);
                                }
                            }
                        }
                    }
                }
            }

            if (!keyValuePairs.TryGetValue("system_Akey", out string? authKey))
            {
                throw new Exception("Нет ключа аутентификации");
            }

            keyValuePairs.Remove("system_Akey");

            var deviceInfo = deviceInfoRepository.Select().FirstOrDefault(d => d.AuthKey == authKey) 
                ?? throw new NotFoundException($"Невалидный ключ аутентификации устройства: {authKey}");

            var point = PointData.Measurement(authKey);

            foreach (var pair in keyValuePairs)
            {
                point = double.TryParse(pair.Value, out double value) ? point.Field(pair.Key, value)
                    : point = point.Field(pair.Key, pair.Value);
            }

            var date = DateTime.UtcNow;

            point = point.Timestamp(date, WritePrecision.Ns);

            using (var writeApi = dbContext.InfluxDBClient.GetWriteApi())
            {
                writeApi.WritePoint(point, dbContext.Bucket, dbContext.Org);
            }

            var fields = keyValuePairs.ToDictionary(r => r.Key, r => (object)r.Value);

            Console.WriteLine(date.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss"));

            return new Device()
            {
                AKey = authKey,
                Date = date.AddHours(3).ToString("yyyy-MM-dd HH:mm:ss"),
                Data = fields
            };
        }
    }
}
