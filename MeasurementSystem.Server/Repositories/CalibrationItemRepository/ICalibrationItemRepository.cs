using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Repositories.CalibrationItemRepository
{
    public interface ICalibrationItemRepository
    {
        IEnumerable<CalibrationItem> Select();
        void Insert(CalibrationItem item);
        void Save();
    }
}
