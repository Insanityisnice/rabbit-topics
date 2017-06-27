using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using consumer.Services;
using consumer.Models;

namespace consumer.Controllers
{
    [Route("api/[controller]")]
    public class ConsumersController : Controller
    {
        private ILogger logger;
        private ConsumerService consumerService;

        public ConsumersController(ILoggerFactory loggerFactory, ConsumerService consumerService)
        {
            this.logger = loggerFactory.CreateLogger(nameof(ConsumersController));
            this.consumerService = consumerService;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return consumerService.Consumers().Select(c => new
                { 
                    Name = c.Name,
                    Exchange = c.Exchange,
                    BindingKey = c.BindingKey,
                });
        }

         [HttpPost]
        public object Post([FromBody] Consumer consumer)
        {
            logger.LogInformation($"Adding new consumer {consumer.Name}.");

            if(consumer == null)
            {
                return BadRequest();
            }

            consumerService.AddConsumer(consumer);
            return new {
                Name = consumer.Name,
                Exchange = consumer.Exchange,
                BindingKey = consumer.BindingKey
            };
        }

        [HttpGet]
        [Route("{consumerName}/messages")]
        public IEnumerable<string> Get(string consumerName)
        {
            using (var scope = logger.BeginScope("GET messages"))
            {
                if(string.IsNullOrWhiteSpace(consumerName)) throw new ArgumentException($"The argument {nameof(consumerName)} is required.", nameof(consumerName));

                return consumerService[consumerName].Messages;
            }
        }
    }
}
