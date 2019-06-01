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
    public class ShopinfoController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("shopinfo.regist")]
        public ActionResult<EamuseXrpcData> Regist([FromBody] EamuseXrpcData data)
        {
            string locationId = data.Document.Element("call").Element("shopinfo").Element("shop").Element("locationid").Value;

            data.Document = new XDocument(new XElement("response", new XElement("shopinfo", new XElement("data",
                new KU32("cabid", 1),
                new KStr("locationid", locationId),
                new KU8("tax_phase", 0),
                new XElement("facility",
                    new KU32("exist", 0)
                ),
                new KU64("event_flag", 0),
                GametopController.GetInfoElement()
            ))));

            return data;
        }
    }
}
