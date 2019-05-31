using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;
using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers
{
    [ApiController, Route("core")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new BadRequestResult();
        }

        [HttpPost, XrpcCall("services.get")]
        public EamuseXrpcData GetServices([FromBody] EamuseXrpcData data, [FromQuery] string model)
        {
            Console.WriteLine("Model: " + model);
            Console.WriteLine();
            Console.WriteLine(data.Data.ToString());

            return data;
        }
    }
}
