using MeasurementSystem.Server.Models;
using System.Collections.Concurrent;

namespace MeasurementSystem.Server.Services
{
    public class MonitoringService
    {
        private readonly ConcurrentQueue<Message> messages;

        public MonitoringService()
        {
            messages = [];
        }

        public void WriteMessage(Message message)
        {
            messages.Enqueue(message);
        }

        public IEnumerable<Message> GetMessages()
        {
            var result = new List<Message>();
            while (!messages.IsEmpty) 
            {
                messages.TryDequeue(out Message? message);

                if (message != null)
                {
                    result.Add(message);
                }
            }
            return result;
        }
    }
}
