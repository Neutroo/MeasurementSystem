using MeasurementSystemWebAPI.Models;

namespace MeasurementSystemWebAPI.Repositories.DeviceRepository
{
    public interface IDeviceRepository
    {
        Task<Dictionary<string, Record>> SelectAsync(DateTime from, DateTime to);
        Task<byte[]> SelectAsBytesAsync(DateTime from, DateTime to);
        void Insert(string device);
    }
}
