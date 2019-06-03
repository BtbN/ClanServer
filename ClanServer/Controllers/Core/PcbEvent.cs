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
    public class PcbEventController : ControllerBase
    {
        [HttpPost, XrpcCall("pcbevent.put")]
        public ActionResult<EamuseXrpcData> Put([FromBody] EamuseXrpcData data)
        {
            // TODO: log these, maybe?
            Console.WriteLine(data.Document);

            data.Document = new XDocument(new XElement("response", new XElement("pcbevent")));
            return data;
        }
    }
}
