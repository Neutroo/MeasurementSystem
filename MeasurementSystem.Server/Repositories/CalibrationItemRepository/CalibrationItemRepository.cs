using MeasurementSystem.Server.Contexts;
using MeasurementSystem.Server.Models;
using System.Data;

namespace MeasurementSystem.Server.Repositories.CalibrationItemRepository
{
    public class CalibrationItemRepository : ICalibrationItemRepository
    {
        private readonly PostgresContext dbContext;

        public CalibrationItemRepository(PostgresContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<CalibrationItem> Select() => dbContext.CalibrationItems.AsEnumerable();

        public void Insert(CalibrationItem item)
        {
            dbContext.CalibrationItems.Add(item);
        }

        public void Save() => dbContext.SaveChanges();
    }
}
