namespace MeasurementSystem.Server.Models
{
    public class Device
    {
        public required string AKey { get; set; }
        public required string Date { get; set; }
        public required IDictionary<string, object> Data { get; set; }
    }
}
