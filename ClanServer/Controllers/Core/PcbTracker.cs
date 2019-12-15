using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers.Core
{
    [ApiController, Route("core")]
    public class PcbTrackerController : ControllerBase
    {
        [HttpPost("{model}/pcbtracker/alive")]
        public ActionResult<EamuseXrpcData> Alive2([FromBody] EamuseXrpcData data, string model)
        {
            return Alive(data, model);
        }

        [HttpPost, XrpcCall("pcbtracker.alive")]
        public ActionResult<EamuseXrpcData> Alive([FromBody] EamuseXrpcData data, [FromQuery] string model)
        {
            data.Document = new XDocument(new XElement("response", new XElement("pcbtracker",
                new XAttribute("ecenable", "1")
            )));

            return data;
        }
    }
}
