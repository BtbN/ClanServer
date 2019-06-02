using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;

using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers.L44
{
    [ApiController, Route("L44")]
    public class JboxController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("jbox.get_list")]
        public ActionResult<EamuseXrpcData> GetList([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("jbox")));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("jbox.get_agreement")]
        public ActionResult<EamuseXrpcData> GetAgreement([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("jbox")));

            return data;
        }
    }
}
