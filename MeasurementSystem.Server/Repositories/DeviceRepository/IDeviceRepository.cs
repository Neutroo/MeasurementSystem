using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Repositories.DeviceRepository
{
    public interface IDeviceRepository
    {
        Task<Dictionary<string, Record>> SelectAsync(DateTime from, DateTime to);
        Task<byte[]> SelectAsBytesAsync(DateTime from, DateTime to);
        Task<Dictionary<string, List<string>>> SelectSensorsByDeviceAsync();
        Task<Dictionary<string, List<string>>> SelectDevicesBySensorAsync();
        Device Insert(string device);
    }
}
