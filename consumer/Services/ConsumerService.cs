using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using consumer.Models;

namespace consumer.Services
{
    public class ConsumerService
    {
        private ILogger logger;
        private ILoggerFactory factory; //Keeping this to give to the consumer.
        private IDictionary<string, Consumer> consumers;

        private ConsumerService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(ConsumerService));
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
            using (var scope = logger.BeginScope(nameof(AddConsumer)))
            {
                if (consumer == null) throw new ArgumentNullException(nameof(consumer));
                
                logger.LogInformation($"Adding a new consumer {consumer.Name} and listening.");
                
                consumer.Start(factory);
                consumers.Add(consumer.Name, consumer);

                logger.LogInformation($"New consumer {consumer.Name} added and listening.");
            }
        }
    }
}