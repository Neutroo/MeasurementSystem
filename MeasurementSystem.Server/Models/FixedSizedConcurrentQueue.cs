using System.Collections.Concurrent;

namespace MeasurementSystem.Server.Models
{
    public class FixedSizedConcurrentQueue<T> : ConcurrentQueue<T>
    {
        private readonly object sync = new();

        public int Capacity { get; private set; }

        public FixedSizedConcurrentQueue(int capacity)
        {
            Capacity = capacity;
        }

        public new void Enqueue(T value)
        {
            base.Enqueue(value);
            lock (sync)
            {
                while (Count > Capacity)
                {
                    base.TryDequeue(out T outValue);
                }
            }
        }
    }
}
