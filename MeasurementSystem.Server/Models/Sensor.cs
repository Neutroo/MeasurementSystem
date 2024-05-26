using Newtonsoft.Json;

namespace MeasurementSystem.Server.Models
{
    public class Sensor
    {
        /// <summary>
        /// Название
        /// </summary>
        [JsonProperty("sensor")]
        public required string Name { get; set; }

        /// <summary>
        /// Калибровки
        /// </summary>
        [JsonProperty("calibr")]
        public required IEnumerable<Calibration> Calibrations { get; set; }
    }
}
