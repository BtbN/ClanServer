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
    public class RecommendController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("recommend.get_recommend")]
        public ActionResult<EamuseXrpcData> GetRecommend([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("get_pdata")));

            return data;
        }
    }
}
