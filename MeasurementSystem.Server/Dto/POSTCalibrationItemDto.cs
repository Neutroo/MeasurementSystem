using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Dto
{
    public class POSTCalibrationItemDto
    {
        public string NameSerial { get; set; }

        public string Sensor { get; set; }

        public double[] Coefficients { get; set; }

        public CalibrationItem ToDomain(string authKey)
            => new()
            {
                Id = Guid.NewGuid(),
                AuthKey = authKey,
                Sensor = Sensor,
                CreationDate = DateTime.UtcNow.AddHours(3),
                Coefficients = Coefficients
            };
    }
}
