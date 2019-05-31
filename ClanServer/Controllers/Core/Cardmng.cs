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
    public class CardmngController : ControllerBase
    {
        [HttpPost, XrpcCall("cardmng.inquire")]
        public ActionResult<EamuseXrpcData> Inquire([FromBody] EamuseXrpcData data)
        {
            string cardid = data.Document.Element("call").Element("cardmng").Attribute("cardid").Value;
            string cardType = data.Document.Element("call").Element("cardmng").Attribute("cardtype").Value;
            string update = data.Document.Element("call").Element("cardmng").Attribute("update").Value;

            if (update != "") // TODO: actually register cards
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("binded", "1"),
                    new XAttribute("dataid", "DD389C3FFB6F47BA"),
                    new XAttribute("refid", "DD389C3FFB6F47BA"),
                    new XAttribute("ecflag", "1"),
                    new XAttribute("newflag", "0"),
                    new XAttribute("expired", "0")
                )));
            }
            else
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("status", "112")
                )));
            }

            return data;
        }

        [HttpPost, XrpcCall("cardmng.authpass")]
        public ActionResult<EamuseXrpcData> Authpass([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("status", "0")
            )));

            return data;
        }
    }
}
