using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers.Core
{
    [ApiController, Route("core")]
    public class FacilityController : ControllerBase
    {
        [HttpPost, XrpcCall("facility.get")]
        public ActionResult<EamuseXrpcData> Get([FromBody] EamuseXrpcData data)
        {
            var facilityReq = data.Document.Element("call").Element("facility");
            string requestedEncoding = facilityReq.Attribute("encoding").Value;
            string method = facilityReq.Attribute("method").Value;

            var Str = new XAttribute("__type", "str");
            var U8 = new XAttribute("__type", "u8");
            var U16 = new XAttribute("__type", "u16");
            var S32 = new XAttribute("__type", "s32");
            var IP4 = new XAttribute("__type", "ip4");

            data.Document = new XDocument(new XElement("response", new XElement("facility",
                new XElement("location",
                    new XElement("id", Str, "53BDC526"),
                    new XElement("country", Str, "US"),
                    new XElement("region", Str, "1"),
                    new XElement("name", Str, ""),
                    new XElement("type", U8, "0")
                ),
                new XElement("line",
                    new XElement("id", Str, ""),
                    new XElement("class", U8, "0")
                ),
                new XElement("portfw",
                    new XElement("globalip", IP4, HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()),
                    new XElement("globalport", U16, "5700"),
                    new XElement("privateport", U16, "5700")
                ),
                new XElement("public",
                    new XElement("flag", U8, "0"),
                    new XElement("name", Str, ""),
                    new XElement("latitude", Str, ""),
                    new XElement("longitude", Str, "")
                ),
                new XElement("share",
                    new XElement("eacoin",
                        new XElement("notchamount", S32, "0"),
                        new XElement("notchcount", S32, "0"),
                        new XElement("supplylimit", S32, "100000")
                    ),
                    new XElement("url",
                        new XElement("eapass", Str, "http://eagate.573.jp/"),
                        new XElement("arcadefan", Str, "http://eagate.573.jp/"),
                        new XElement("konaminetdx", Str, "http://eagate.573.jp/"),
                        new XElement("konamiid", Str, "http://eagate.573.jp/"),
                        new XElement("eagate", Str, "http://eagate.573.jp/")
                    )
                )
            )));

            if (requestedEncoding == "Shift-JIS")
            {
                data.Encoding = Encoding.GetEncoding(932);
            }
            else
            {
                Console.WriteLine("Unknown encoding requested, ignoring.");
            }

            return data;
        }
    }
}
