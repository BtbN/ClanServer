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
    [ApiController, Route("L44")]
    public class ShopinfoController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("shopinfo.regist")]
        public ActionResult<EamuseXrpcData> Regist([FromBody] EamuseXrpcData data)
        {
            string locationId = data.Document.Element("call").Element("shopinfo").Element("shop").Element("locationid").Value;

            var Str = new XAttribute("__type", "str");
            var U8 = new XAttribute("__type", "u8");
            var S16 = new XAttribute("__type", "s16");
            var S32 = new XAttribute("__type", "s32");
            var U32 = new XAttribute("__type", "u32");
            var U64 = new XAttribute("__type", "u64");
            var Bool = new XAttribute("__type", "bool");

            var C16 = new XAttribute("__count", "16");
            var C64 = new XAttribute("__count", "64");

            data.Document = new XDocument(new XElement("response", new XElement("shopinfo", new XElement("data",
                new XElement("cabid", U32, "1"),
                new XElement("locationid", Str, locationId),
                new XElement("tax_phase", U8, "0"),
                new XElement("facility",
                    new XElement("exist", U32, "0")
                ),
                new XElement("event_flag", U64, "0"),
                new XElement("info",
                    new XElement("event_info",
                        new XElement("event", new XAttribute("type", "15"),
                            new XElement("state", U8, "1")
                        ),
                        new XElement("event", new XAttribute("type", "5"),
                            new XElement("state", U8, "0")
                        ),
                        new XElement("event", new XAttribute("type", "6"),
                            new XElement("state", U8, "0")
                        )
                    ),
                    new XElement("share_music"),
                    new XElement("genre_def_music"),
                    new XElement("black_jacket_list", S32, C64, "0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0"),
                    new XElement("white_music_list", S32, C64, "-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1"),
                    new XElement("white_marker_list", S32, C16, "-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1"),
                    new XElement("white_theme_list", S32, C16, "-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1"),
                    new XElement("open_music_list", S32, C64, "-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1"),
                    new XElement("shareable_music_list", S32, C64, "-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1"),
                    new XElement("jbox",
                        new XElement("point", S32, "1"),
                        new XElement("emblem",
                            new XElement("normal",
                                new XElement("index", S16, "50")
                            ),
                            new XElement("premium",
                                new XElement("index", S16, "50")
                            )
                        )
                    ),
                    new XElement("collection",
                        new XElement("rating_s")
                    ),
                    new XElement("expert_option",
                        new XElement("is_available", Bool, "1")
                    ),
                    new XElement("all_music_matching",
                        new XElement("is_available", Bool, "0")
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
