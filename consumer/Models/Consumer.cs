using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace consumer.Models
{
    public class Consumer
    {
        public string Name { get; private set; }
        public string Exchange  { get; private set; }
        public string BindingKey  { get; private set; }
        
        private ILogger logger;
        private IList<string> messages = new List<string>();
        private BackgroundWorker bgWorker;

        public IEnumerable<string> Messages
        {
            get { return messages; }
        }

        public Consumer(string name, string exchange, string bindingKey)
        {
            Name = name;
            Exchange = exchange;
            BindingKey = bindingKey;
        }

        public void Start(ILoggerFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            logger = factory.CreateLogger($"Consumer:{Name}");

            using (var scope = logger.BeginScope(nameof(Start)))
            {
                try
                {
                logger.LogInformation($"Creating worker for {Name}.");
                bgWorker = new BackgroundWorker();
                bgWorker.WorkerSupportsCancellation = true;
                bgWorker.DoWork += new DoWorkEventHandler(Listen);

                logger.LogInformation($"Starting {Name}:worker.");
                bgWorker.RunWorkerAsync(this);
                }
                catch(Exception ex)
                {
                    logger.LogError(new EventId(0), ex, ex.Message);
                    throw;
                }
            }
        }

        public void Stop()
        {
            using (var scope = logger.BeginScope(nameof(Stop)))
            {
                logger.LogInformation($"Stoping {Name}:worker.");
                bgWorker.CancelAsync();
            }
        }

        private void Listen(object sender, DoWorkEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (!(sender is BackgroundWorker)) throw new ArgumentException($"The sender must be of type {nameof(BackgroundWorker)}.", nameof(sender)); 
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!(e.Argument is Consumer)) throw new ArgumentException($"The event argument must be of type {nameof(Consumer)}.", nameof(e));


            BackgroundWorker worker = sender as BackgroundWorker;
            Consumer cthis = (Consumer)e.Argument;
            ILogger logger = cthis.logger;

            string exchange = cthis.Exchange;
            string bindingKey = cthis.BindingKey;

            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_URI"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    logger.LogDebug($"Declaring exchange {exchange} for {Name}:worker.");
                    channel.ExchangeDeclare(exchange: exchange, type: "topic");

                    logger.LogDebug($"Declaring queue for {Name}:worker.");
                    var queueName = channel.QueueDeclare().QueueName;
                    logger.LogDebug($"{queueName} declared for {Name}:worker.");

                    channel.QueueBind(queue: queueName,
                            exchange: exchange,
                            routingKey: bindingKey);
                    logger.LogDebug($"{Name}:worker is bound to queue {queueName}.");

                    var consumer = new EventingBasicConsumer(channel);
                    
                    consumer.Received += (model, ea) =>
                    {
                        using (var scope = logger.BeginScope($"Recieved {Name}:worker"))
                        {
                            try
                            {
                                logger.LogDebug($"Message received {Name}:worker");
                                var body = ea.Body;
                                var message = Encoding.UTF8.GetString(body);
                                var routingKey = ea.RoutingKey;
                            
                                messages.Add(message);
                                logger.LogDebug($"Message stored {Name}:worker");
                            }
                            catch(Exception ex)
                            {
                                logger.LogDebug(ea.ToString());
                                logger.LogWarning(new EventId(0), ex, ex.Message);
                                throw;
                            }
                        }
                    };

                    channel.BasicConsume(queue: queueName,
                                        noAck: true,
                                        consumer: consumer);
                    logger.LogInformation($"{Name}:worker is waiting for messages on exchange:{exchange} and queue:{queueName}.");

                    while(!worker.CancellationPending)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                    e.Cancel = true;
                }
            }
        }
    }
}