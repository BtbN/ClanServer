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
    [ApiController, Route("L44")]
    public class DemodataController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("demodata.get_info")]
        public ActionResult<EamuseXrpcData> GetInfo([FromBody] EamuseXrpcData data)
        {
            var S32 = new XAttribute("__type", "s32");
            var C64 = new XAttribute("__count", "64");

            data.Document = new XDocument(new XElement("response", new XElement("demodata", new XElement("data", new XElement("info", 
                new XElement("black_jacket_list", S32, C64, "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0")
            )))));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("demodata.get_news")]
        public ActionResult<EamuseXrpcData> GetNews([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("demodata", new XElement("data",
                new XElement("officialnews", new XAttribute("count", 0))
            ))));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("demodata.get_jbox_list")]
        public ActionResult<EamuseXrpcData> GetJboxList([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("demodata")));

            return data;
        }
    }
}
