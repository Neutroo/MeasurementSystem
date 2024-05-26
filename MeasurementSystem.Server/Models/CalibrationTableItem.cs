using Newtonsoft.Json;

namespace MeasurementSystem.Server.Models
{
    public class CalibrationTableItem
    {
        /// <summary>
        /// Имя прибора
        /// </summary>
        [JsonProperty("uName")]
        public required string DeviceName { get; set; }

        /// <summary>
        /// Серийный номер прибора
        /// </summary>
        [JsonProperty("serial")]
        public required string? DeviceSerial { get; set; }

        /// <summary>
        /// Датчики
        /// </summary>
        [JsonProperty("sensors")]
        public required IEnumerable<Sensor> Sensors { get; set; }
    }
}
