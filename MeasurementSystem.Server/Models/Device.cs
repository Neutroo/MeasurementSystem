namespace MeasurementSystem.Server.Models
{
    public class Device
    {
        public required string AKey { get; set; }
        public required string Date { get; set; }
        public required Dictionary<string, string> Fields { get; set; }
    }
}
