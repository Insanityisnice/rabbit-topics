using System;
using System.Collections.Generic;

namespace consumer.Controllers
{
    public class ConsumerService
    {
        private IDictionary<string, Consumer> consumers;

        public static ConsumerService Instance;

        static ConsumerService()
        {
            Instance = new ConsumerService();
        }

        private ConsumerService()
        {
            consumers = new Dictionary<string, Consumer>();
        }

        public object this[string index]
        {
            get 
            {
                if (!consumers.ContainsKey(index)) throw new ArgumentOutOfRangeException(nameof(index), $"The consumer named {index} could not be found.");
                
                return consumers[index];
            }
        }

        public void AddConsumer(string name, string exchange, string bindingKey)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("", nameof(name));
            if (consumers.ContainsKey(name)) throw new ArgumentException(nameof(name), $"The consumer named {name} already exists.");
            if (String.IsNullOrWhiteSpace(exchange)) throw new ArgumentException("", nameof(exchange));
            if (String.IsNullOrWhiteSpace(bindingKey)) throw new ArgumentException("", nameof(bindingKey));
            
            consumers.Add(name, new Consumer(exchange, bindingKey));
        }
    }

    public class Consumer
    {
        private IList<string> messages = new List<string>();

        public Consumer(string exchange, string bindingKey)
        {
        }
    }
}