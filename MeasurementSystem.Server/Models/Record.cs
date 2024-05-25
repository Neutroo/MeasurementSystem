using Newtonsoft.Json;

namespace MeasurementSystem.Server.Models
{
    public class Record
    {
        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("uName")]
        public string DeviceName { get; set; }

        [JsonProperty("serial")]
        public string? DeviceSerial { get; set; }

        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; set; }
    }
}
