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
    public class ServicesController : ControllerBase
    {
        [HttpPost, XrpcCall("services.get")]
        public ActionResult<EamuseXrpcData> Get([FromBody] EamuseXrpcData data, [FromQuery] string model)
        {
            string url = Request.Scheme + "://" + Request.Host.Host + ":" + (Request.Host.Port ?? Startup.Port);
            string coreUrl = url + "/core";
            string modelUrl;
            string[] modelItems;

            string[] coreItems = new[]
{
                "cardmng",
                "facility",
                "message",
                "numbering",
                "package",
                "pcbevent",
                "pcbtracker",
                "pkglist",
                "posevent",
                "userdata",
                "userid",
                "eacoin",
            };

            if (model.StartsWith("L44:J:E:A:2018"))
            {
                modelItems = new[]
                {
                    "local",
                    "local2",
                    "lobby",
                    "lobby2"
                };

                modelUrl = url + "/L44/8";
            }
            else
            {
                return NotFound();
            }

            XElement servicesElement = new XElement("services",
                new XAttribute("expire", "600"),
                new XAttribute("method", "get"),
                new XAttribute("mode", "operation"),
                new XAttribute("status", "0"));

            foreach (string coreItem in coreItems)
                servicesElement.Add(new XElement("item", new XAttribute("name", coreItem), new XAttribute("url", coreUrl)));

            foreach (string modelItem in modelItems)
                servicesElement.Add(new XElement("item", new XAttribute("name", modelItem), new XAttribute("url", modelUrl)));

            servicesElement.Add(new XElement("item", new XAttribute("name", "ntp"), new XAttribute("url", "ntp://pool.ntp.org/")));
            servicesElement.Add(new XElement("item", new XAttribute("name", "keepalive"), new XAttribute("url", "http://127.0.0.1/keepalive?pa=127.0.0.1&ia=127.0.0.1&ga=127.0.0.1&ma=127.0.0.1&t1=2&t2=10")));

            data.Document = new XDocument(new XElement("response", servicesElement));

            return data;
        }
    }
}