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
    public class PackageController : ControllerBase
    {
        [HttpPost("{model}/package/list")]
        public ActionResult<EamuseXrpcData> List2([FromBody] EamuseXrpcData data)
        {
            return List(data);
        }

        [HttpPost, XrpcCall("package.list")]
        public ActionResult<EamuseXrpcData> List([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("package")));

            return data;
        }
    }
}
