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
    public class LobbyController : ControllerBase
    {
        [HttpPost, Route("8"), XrpcCall("lobby.check")]
        public ActionResult<EamuseXrpcData> Check([FromBody] EamuseXrpcData data)
        {
            data.Document = new XDocument(new XElement("response", new XElement("lobby", new XElement("data",
                new KU32("entrant_nr", 1).AddAttr("time", 0),
                new KS16("interval", 60),
                new KS16("entry_timeout", 30),
                new XElement("waitlist", new XAttribute("count", 0))
            ))));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("lobby.entry")]
        public ActionResult<EamuseXrpcData> Entry([FromBody] EamuseXrpcData data)
        {
            XElement lobby = data.Document.Element("call").Element("lobby");
            XElement music = lobby.Element("data").Element("music");

            Random rng = new Random();
            byte[] buf = new byte[8];
            rng.NextBytes(buf);
            long roomId = BitConverter.ToInt64(buf, 0);

            uint musicId = uint.Parse(music.Element("id").Value);
            byte seq = byte.Parse(music.Element("seq").Value);

            data.Document = new XDocument(new XElement("response", new XElement("lobby",
                new XElement("data",
                    new KS64("roomid", roomId).AddAttr("master", 1),
                    new KS16("refresh_intr", 2),
                    new XElement("music",
                        new KU32("id", musicId),
                        new KU8("seq", seq)
                    )
                )
            )));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("lobby.refresh")]
        public ActionResult<EamuseXrpcData> Refresh([FromBody] EamuseXrpcData data)
        {
            XElement lobby = data.Document.Element("call").Element("lobby");
            _ = long.Parse(lobby.Element("data").Element("roomid").Value);

            data.Document = new XDocument(new XElement("response", new XElement("lobby",
                new XElement("data",
                    new KS16("refresh_intr", 2),
                    new KBool("start", true)
                )
            )));

            return data;
        }

        [HttpPost, Route("8"), XrpcCall("lobby.report")]
        public ActionResult<EamuseXrpcData> Report([FromBody] EamuseXrpcData data)
        {
            XElement lobby = data.Document.Element("call").Element("lobby");
            _ = long.Parse(lobby.Element("data").Element("roomid").Value);

            data.Document = new XDocument(new XElement("response", new XElement("lobby",
                new XElement("data",
                    new KS16("refresh_intr", 2)
                )
            )));

            return data;
        }
    }
}
