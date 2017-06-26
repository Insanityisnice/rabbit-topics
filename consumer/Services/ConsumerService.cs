using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using consumer.Models;

namespace consumer.Services
{
    public class ConsumerService
    {
        private IDictionary<string, Consumer> consumers;

        private ConsumerService()
        {
            consumers = new ConcurrentDictionary<string, Consumer>();
        }

        public Consumer this[string index]
        {
            get 
            {
                if (!consumers.ContainsKey(index)) throw new ArgumentOutOfRangeException(nameof(index), $"The consumer named {index} could not be found.");
                
                return consumers[index];
            }
        }

        public IEnumerable<Consumer> Consumers()
        {
            return consumers.Values;
        }

        public void AddConsumer(Consumer consumer)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));
            
            consumer.Start();
            consumers.Add(consumer.Name, consumer);
        }
    }
}