using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

namespace producer.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private ILogger logger;

        public MessagesController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(nameof(MessagesController));
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "Success!";
        }

        // POST api/messages
        [HttpPost]
        [Route("{exchange}/{routingKey}")]
        public void Post(string exchange, string routingKey, [FromBody]string message)
        {
            using (var source = logger.BeginScope($"Post {exchange}:{routingKey}"))
            {
                logger.LogDebug($"Queuing message for {exchange}:{routingKey}");
                logger.LogDebug($"Message is {message}.");

                try
                {
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
                            
                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: exchange,
                                            routingKey: routingKey,
                                            basicProperties: null,
                                            body: body);
                            
                            logger.LogDebug($"Sent '{exchange}' -'{routingKey}':'{message}'");
                        }
                    }
                }
                catch(Exception ex)
                {
                    logger.LogError(new EventId(1), ex, ex.Message);
                    throw ex;
                }
            }
        }
    }
}
