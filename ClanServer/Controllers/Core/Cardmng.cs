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
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            string cardid = cardmng.Attribute("cardid").Value;
            string cardType = cardmng.Attribute("cardtype").Value;
            string update = cardmng.Attribute("update").Value;

            if (update != "") // TODO: actually register cards
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("binded", "1"),
                    new XAttribute("dataid", "DD389C3FFB6F47BA"),
                    new XAttribute("ecflag", "1"),
                    new XAttribute("newflag", "0"),
                    new XAttribute("expired", "0"),
                    new XAttribute("refid", "DD389C3FFB6F47BA")
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
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            string pass = cardmng.Attribute("pass").Value;
            string refId = cardmng.Attribute("refid").Value;

            //TODO

            data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("status", "0")
            )));

            return data;
        }

        [HttpPost, XrpcCall("cardmng.getrefid")]
        public ActionResult<EamuseXrpcData> GetRefId([FromBody] EamuseXrpcData data)
        {
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            //TODO: register new user

            Console.WriteLine(data.Document);

            data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("dataid", "DD389C3FFB6F47BA"),
                    new XAttribute("refid", "DD389C3FFB6F47BA")
            )));

            return data;
        }
    }
}
