using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public void Start()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(Listen);

            bgWorker.RunWorkerAsync(this);
        }

        public void Stop()
        {
            bgWorker.CancelAsync();
        }

        private void Listen(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Consumer cthis = (Consumer)e.Argument;

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
                    channel.ExchangeDeclare(exchange: exchange, type: "topic");
                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                            exchange: exchange,
                            routingKey: bindingKey);

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine($" [x] Received '{routingKey}':'{message}'");
                    };

                    channel.BasicConsume(queue: queueName,
                                        noAck: true,
                                        consumer: consumer);

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