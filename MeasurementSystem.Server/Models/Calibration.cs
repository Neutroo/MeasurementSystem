using Newtonsoft.Json;

namespace MeasurementSystem.Server.Models
{
    public class Calibration
    {
        /// <summary>
        /// Время создания
        /// </summary>
        [JsonProperty("datetime")]
        public required string CreationDate { get; set; }

        /// <summary>
        /// Коэффициенты
        /// </summary>
        [JsonProperty("data")]
        public required object Data { get; set; }
    }
}
