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
    [Route("api/consumers/[consumerName]/[controller]")]
    public class MessagesController : Controller
    {
        private ILogger logger;
        private ConsumerService consumerService;

        public MessagesController(ILoggerFactory loggerFactory, ConsumerService consumerService)
        {
            this.logger = loggerFactory.CreateLogger(nameof(MessagesController));
            this.consumerService = consumerService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(string consumerName)
        {
            if(string.IsNullOrWhiteSpace(consumerName)) throw new ArgumentException($"The argument {nameof(consumerName)} is required.", nameof(consumerName));

            return consumerService[consumerName].Messages;
        }
    }
}
