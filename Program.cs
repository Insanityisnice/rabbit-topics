using System;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE");
            var bindingKey = Environment.GetEnvironmentVariable("RABBITMQ_BINDINGKEY");

            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_URI"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
            };

            //HACK: Wait for rabbit container to spin up.
            System.Threading.Thread.Sleep(15000);

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

                    while(true);
                    Console.ReadLine();
                }
            }
        }
    }
}
