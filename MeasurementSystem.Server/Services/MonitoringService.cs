using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Services
{
    public class MonitoringService
    {
        private readonly FixedSizedConcurrentQueue<Message> messages;
        private const int MESSAGE_POOL_CAPACITY = 10;

        public MonitoringService()
        {
            messages = new FixedSizedConcurrentQueue<Message>(MESSAGE_POOL_CAPACITY);
        }

        public void WriteMessage(Message message)
        {
            messages.Enqueue(message);
        }

        public IEnumerable<Message> GetMessages()
        {
            return messages.ToArray();
        }
    }
}
