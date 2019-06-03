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
    public class LoggerController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("logger.report")]
        public ActionResult<EamuseXrpcData> Report([FromBody] EamuseXrpcData data)
        {
            XElement dataE = data.Document.Element("call").Element("logger").Element("data");

            string srcId = data.Document.Element("call").Attribute("srcid").Value;
            string code = dataE.Element("code").Value;
            string info = dataE.Element("information").Value;

            Console.WriteLine("[" + srcId + ", " + code + "]:" + info);

            data.Document = new XDocument(new XElement("response", new XElement("logger")));

            return data;
        }
    }
}
