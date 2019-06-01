using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;

using ClanServer.Routing;

namespace ClanServer.Controllers.L44
{
    [ApiController, Route("L44")]
    public class LobbyController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("lobby.check")]
        public ActionResult<EamuseXrpcData> Check([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("lobby")));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("lobby.entry")]
        public ActionResult<EamuseXrpcData> Entry([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("lobby")));

            return data;
        }
    }
}
