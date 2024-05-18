using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Repositories.DeviceRepository
{
    public interface IDeviceRepository
    {
        Task<Dictionary<string, Record>> SelectAsync(DateTime from, DateTime to);
        Task<byte[]> SelectAsBytesAsync(DateTime from, DateTime to);
        Task<Dictionary<string, IEnumerable<string>>> SelectDeviceFieldsAsync();
        Device Insert(string device);
    }
}
