using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;

using ClanServer.Routing;
using ClanServer.Data.L44;

namespace ClanServer.Controllers.L44
{
    [ApiController, Route("L44")]
    public class DemodataController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("demodata.get_info")]
        public ActionResult<EamuseXrpcData> GetInfo([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("demodata", new XElement("data", new XElement("info",
                new KS32("black_jacket_list", 64, 0)
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

        [HttpPost, Route("8"), XrpcCall("demodata.get_hitchart")]
        public async Task<ActionResult<EamuseXrpcData>> GetHitchart([FromBody] EamuseXrpcData data)
        {
            ClanMusicInfo mInfo = await ClanMusicInfo.Instance;

            List<int> hitChart = mInfo.GetRandomSongs(10);

            XElement orgElem = new XElement("hitchart_org", new XAttribute("count", hitChart.Count));

            for (short i = 0; i < hitChart.Count; ++i)
            {
                orgElem.Add(new XElement("rankdata",
                    new KS32("music_id", hitChart[i]),
                    new KS16("rank", i),
                    new KS16("prev", i)
                ));
            }

            data.Document = new XDocument(new XElement("response", new XElement("demodata",
                new XElement("data",
                    new KStr("update", "1"),
                    new XElement("hitchart_lic", new XAttribute("count", 0)),
                    orgElem
                )
            )));

            return data;
        }
    }
}
