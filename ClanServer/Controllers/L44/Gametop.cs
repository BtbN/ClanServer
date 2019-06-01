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
    public class GametopController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("gametop.regist")]
        public ActionResult<EamuseXrpcData> Register([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("gametop")));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("gametop.get_pdata")]
        public ActionResult<EamuseXrpcData> GetPdata([FromBody] EamuseXrpcData data)
        {
            var gametop = data.Document.Element("call").Element("gametop");
            var player = gametop.Element("data").Element("player");

            string refId = player.Element("refid").Value;

            data.Document = new XDocument(new XElement("response", new XElement("gametop",
                new XElement("data",
                    GetInfoElement(),
                    GetPlayerElement(refId)
                )
            )));

            return data;
        }

        public static XElement GetInfoElement()
        {
            return new XElement("info",
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
            );
        }

        private XElement GetPlayerElement(string refId)
        {
            int[] musicList = new int[64]
            {
                -2013265951, -102760493, 1711275733, -1579088899,
                -108536, -227069, -33554401, 16383,
                0, -1377473, -402653185, -2097153,
                -1231036417, -786433, -444727297, -1,
                980541439, -33357824, 1077928957, 133988323,
                1075838048, -32706, -234907777, -196609,
                33138687, -2097152, -907557381, -2,
                -134217841, -34734081, -524293, -1641628417,
                -1, -1, -2177, -7532097,
                -3, 264241151, 2080768, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 0
            };

            int[] secretList = musicList;
            int[] themeList = new int[16] { 1023, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] markerList = new int[16] { -1, 7167, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int[] titleList = new int[160];
            titleList[0] = 1;
            titleList[5] = 65536;
            titleList[11] = 1024;
            titleList[12] = 1048576;

            int[] partsList = new int[160];
            partsList[0] = 1;
            partsList[58] = -2147483648;
            partsList[60] = 32;

            int[] emblemList = new int[96];
            emblemList[0] = 1;
            emblemList[7] = 16;
            emblemList[13] = 536870916;
            emblemList[16] = 65536;
            emblemList[18] = 4194304;
            emblemList[22] = 268435456;
            emblemList[24] = 65536;
            emblemList[25] = 8388624;
            emblemList[33] = 147456;
            emblemList[37] = 4194312;
            emblemList[38] = 33554432;
            emblemList[39] = 205520896;
            emblemList[42] = 2080;
            emblemList[43] = 2;
            emblemList[45] = 512;

            int[] commuList = markerList;

            return new XElement("player",
                new KS32("jid", 612645048),
                new KS32("session_id", 1),
                new KStr("name", "CLANTEST"),
                new KU64("event_flag", 2),
                new XElement("info",
                    new KS32("tune_cnt", 991),
                    new KS32("save_cnt", 0),
                    new KS32("saved_cnt", 45),
                    new KS32("fc_cnt", 40),
                    new KS32("ex_cnt", 0),
                    new KS32("clear_cnt", 946),
                    new KS32("match_cnt", 0),
                    new KS32("beat_cnt", 0),
                    new KS32("mynews_cnt", 0),
                    new KS32("mtg_entry_cnt", 0),
                    new KS32("mtg_hold_cnt", 0),
                    new KU8("mtg_result", 0),
                    new KS32("bonus_tune_points", 320),
                    new KBool("is_bonus_tune_played", true)
                ),
                new XElement("last",
                    new KS64("play_time", 0),
                    new KStr("shopname", "x"),
                    new KStr("areaname", "x"),
                    new KS32("music_id", 70000183),
                    new KS8("seq_id", 1),
                    new KStr("seq_edit_id", ""),
                    new KS8("sort", 3),
                    new KS8("category", 24),
                    new KS8("expert_option", 0),
                    new XElement("settings",
                        new KS8("marker", 3),
                        new KS8("theme", 6),
                        new KS16("title", 0),
                        new KS16("parts", 0),
                        new KS8("rank_sort", 1),
                        new KS8("combo_disp", 1),
                        new KS16("emblem", new short[] { 0, 823, 0, 0, 0 }),
                        new KS8("matching", 0),
                        new KS8("hard", 0),
                        new KS8("hazard", 0)
                    )
                ),
                new XElement("item",
                    new KS32("music_list", musicList),
                    new KS32("secret_list", secretList),
                    new KS32("theme_list", themeList),
                    new KS32("marker_list", markerList),
                    new KS32("title_list", titleList),
                    new KS32("parts_list", partsList),
                    new KS32("emblem_list", emblemList),
                    new KS32("commu_list", commuList),
                    new XElement("new",
                        new KS32("secret_list", 64, 0),
                        new KS32("theme_list", 16, 0),
                        new KS32("marker_list", 16, 0)
                    )
                ),
                new XElement("team", new XAttribute("id", "1"),
                    new KS32("section", 4),
                    new KS32("street", 6),
                    new KS32("house_number_1", 67),
                    new KS32("house_number_2", 1),
                    new XElement("move",
                        new XAttribute("house_number_1", 67),
                        new XAttribute("house_number_2", 1),
                        new XAttribute("id", 1),
                        new XAttribute("section", 4),
                        new XAttribute("street", 6)
                    )
                ),
                new XElement("jbox",
                    new KS32("point", 700),
                    new XElement("emblem",
                        new XElement("normal",
                            new KS16("index", 1182)
                        ),
                        new XElement("premium",
                            new KS16("index", 1197)
                        )
                    )
                ),
                new XElement("news",
                    new KS16("checked", 0),
                    new KU32("checked_flag", 1)
                ),
                new XElement("free_first_play",
                    new KBool("is_available", false)
                ),
                new XElement("event_info",
                    new XElement("event", new XAttribute("type", "15"),
                        new KU8("state", 1)
                    )
                ),
                new XElement("new_music"),
                new XElement("gift_list"),
                new XElement("jubility", new XAttribute("param", "0"),
                    new XElement("target_music_list")
                ),
                new XElement("born",
                    new KS8("status", 1),
                    new KS16("year", 2000)
                ),
                new XElement("question_list"),
                new XElement("official_news",
                    new XElement("news_list")
                ),
                GenDroplist(),
                new XElement("daily_bonus_list"),
                GenClanCourseList(),
                new XElement("server"),
                new XElement("rivallist"),
                new XElement("fc_challenge",
                    new XElement("today",
                        new KS32("music_id", 40000057),
                        new KU8("state", 0)
                    ),
                    new XElement("whim",
                        new KS32("music_id", 30000041),
                        new KU8("state", 80)
                    )
                ),
                new XElement("navi",
                    new KU64("flag", 122)
                )
            );
        }

        private static XElement GenClanCourseList()
        {
            XElement categoryList = new XElement("category_list");

            for (int i = 1; i <= 6; ++i)
            {
                categoryList.Add(new XElement("category", new XAttribute("id", i),
                    new KBool("is_display", true)
                ));
            }

            return new XElement("clan_course_list",
                new XElement("clan_course", new XAttribute("id", "50"),
                    new KS8("status", 1)
                ),
                categoryList
            );
        }

        private static XElement GenDroplist()
        {
            var res = new XElement("drop_list");
            var item_list = new XElement("item_list");

            for (int i = 1; i <= 10; ++i)
            {
                item_list.Add(new XElement("item", new XAttribute("id", i),
                    new KS32("num", 0)
                ));
            }

            for (int i = 1; i <= 10; ++i)
            {
                res.Add(new XElement("drop", new XAttribute("id", i),
                    new KS32("exp", 0),
                    new KS32("flag", 0),
                    item_list
                ));
            }

            return res;
        }

        [HttpPost, Route("8"), XrpcCall("gametop.get_mdata")]
        public ActionResult<EamuseXrpcData> GetMdata([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("gametop")));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("gametop.get_meeting")]
        public ActionResult<EamuseXrpcData> GetMeeting([FromBody] EamuseXrpcData data)
        {
            //TODO
            data.Document = new XDocument(new XElement("response", new XElement("gametop")));

            return data;
        }
    }
}
