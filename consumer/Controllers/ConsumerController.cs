using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace consumer.Controllers
{
    [Route("api/[controller]")]
    public class ConsumerController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
        }
    }
}
