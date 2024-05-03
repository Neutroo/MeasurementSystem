using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Repositories.DeviceInfoRepository
{
    public interface IDeviceInfoRepository
    {
        IEnumerable<DeviceInfo> Select();
        DeviceInfo Select(int id);
        void Insert(DeviceInfo deviceInfo);
        void Update(int id, Dictionary<string, object> items);
        void Delete(int id);
        void Save();
    }
}
