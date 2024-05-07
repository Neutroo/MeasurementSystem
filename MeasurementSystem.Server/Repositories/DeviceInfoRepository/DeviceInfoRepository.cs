using InfluxDB.Client.Core.Exceptions;
using MeasurementSystem.Server.Contexts;
using MeasurementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MeasurementSystem.Server.Repositories.DeviceInfoRepository
{
    public class DeviceInfoRepository : IDeviceInfoRepository
    {
        private readonly PostgresContext dbContext;

        public DeviceInfoRepository(PostgresContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<DeviceInfo> Select() => dbContext.DeviceInfos.AsEnumerable();

        public DeviceInfo Select(int id)
        {
            var deviceInfo = dbContext.DeviceInfos.Find(id) ?? throw new NotFoundException("Нет объекта с таким id");
            return deviceInfo;
        }

        public void Insert(DeviceInfo deviceInfo)
        {
            if (dbContext.DeviceInfos.Any(i => i.AuthKey == deviceInfo.AuthKey.Trim()))
            {
                throw new DuplicateNameException($"Прибор с таким ключом уже существует");
            }

            dbContext.DeviceInfos.Add(deviceInfo);
        }

        public void Update(int id, Dictionary<string, object> items)
        {
            var info = dbContext.DeviceInfos.Find(id) ?? throw new NotFoundException("Нет объекта с таким id");;
            
            foreach (var item in items)
            {
                switch (item.Key)
                {
                    case "name":
                        if (item.Value?.ToString() is string name)
                        {
                            info.Name = name;
                        }
                        break;
                    case "serial":
                        info.Serial = item.Value!.ToString();
                        break;
                    case "authKey":
                        if (item.Value?.ToString() is string authKey)
                        {
                            info.AuthKey = authKey;
                        }
                        break;
                    case "x":
                        if (double.TryParse(item.Value?.ToString(), out var x))
                        {
                            info.X = x;
                        }
                        else
                        {
                            throw new InvalidCastException($"Нельзя привести значение {item.Value} к типу double");
                        }
                        break;
                    case "y":
                        if (double.TryParse(item.Value?.ToString(), out var y))
                        {
                            info.Y = y;
                        }
                        else
                        {
                            throw new InvalidCastException($"Нельзя привести значение {item.Value} к типу double");
                        }
                        break;
                    case "location":
                        if (item.Value?.ToString() is string location)
                        {
                            info.Location = location;
                        }
                        break;
                    case "isDeleted":
                        if (bool.TryParse(item.Value?.ToString(), out var isDeleted))
                        {
                            info.IsDeleted = isDeleted;
                        }
                        else
                        {
                            throw new InvalidCastException($"Нельзя привести значение {item.Value} к типу bool");
                        }
                        break;
                    default:
                        throw new NotFoundException($"Нет такого поля для обновления: {item.Key}");
                }
            }
        }

        public void Delete(int id)
        {
            var info = dbContext.DeviceInfos.Find(id);
            if (info != null)
            {
                dbContext.DeviceInfos.Remove(info);
            }
        }

        public void Save() => dbContext.SaveChanges();
    }
}
