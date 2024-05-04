namespace MeasurementSystem.Server.Models
{
    public class Message
    {
        public required string Type { get; set; }
        public required object Content { get; set; }
    }
}
