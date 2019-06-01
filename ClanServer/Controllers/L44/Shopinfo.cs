using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

using eAmuseCore.KBinXML;

using ClanServer.Formatters;
using ClanServer.Routing;

namespace ClanServer.Controllers.Core
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
                new XElement("info",
                    new XElement("event_info",
                        new XElement("event", new XAttribute("type", "15"),
                            new KU8("state", 1)
                        ),
                        new XElement("event", new XAttribute("type", "5"),
                            new KU8("state", 0)
                        ),
                        new XElement("event", new XAttribute("type", "6"),
                            new KU8("state", 0)
                        )
                    ),
                    new XElement("share_music"),
                    new XElement("genre_def_music"),
                    new KS32("black_jacket_list", 64, 0),
                    new KS32("white_music_list", 64, -1),
                    new KS32("white_marker_list", 16, -1),
                    new KS32("white_theme_list", 16, -1),
                    new KS32("open_music_list", 64, -1),
                    new KS32("shareable_music_list", 64, -1),
                    new XElement("jbox",
                        new KS32("point", 1),
                        new XElement("emblem",
                            new XElement("normal",
                                new KS16("index", 50)
                            ),
                            new XElement("premium",
                                new KS16("index", 50)
                            )
                        )
                    ),
                    new XElement("collection",
                        new XElement("rating_s")
                    ),
                    new XElement("expert_option",
                        new KBool("is_available", true)
                    ),
                    new XElement("all_music_matching",
                        new KBool("is_available", false)
                    ),
                    new XElement("department",
                        new XElement("pack_list")
                    )
                )
            ))));

            return data;
        }
    }
}
