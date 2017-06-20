using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace producer.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "Success!";
        }

        // POST api/messages
        [HttpPost]
        [Route("{exchange}/{queue}/{routeKey}")]
        public string Post(string exchange, string queue, string routeKey, [FromBody]string message)
        {
            return $"{exchange}:{queue}:{routeKey} - Success!";
        }
    }
}
