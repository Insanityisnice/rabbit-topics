using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace consumer.Controllers
{
    [Route("api/consumer/[consumerName][controller]")]
    public class MessagesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(string consumerName)
        {
            if(string.IsNullOrWhiteSpace(consumerName)) throw new ArgumentException($"The argument {nameof(consumerName)} is required.", nameof(consumerName));
            
            return ConsumerService.Instance[consumerName].Messages;
        }
    }
}
