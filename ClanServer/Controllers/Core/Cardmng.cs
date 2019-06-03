using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ClanServer.Helpers;
using ClanServer.Routing;
using ClanServer.Models;

namespace ClanServer.Controllers.Core
{
    [ApiController, Route("core")]
    public class CardmngController : ControllerBase
    {
        private readonly ClanServerContext ctx;

        public CardmngController(ClanServerContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpPost, XrpcCall("cardmng.inquire")]
        public ActionResult<EamuseXrpcData> Inquire([FromBody] EamuseXrpcData data)
        {
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            byte[] cardId = cardmng.Attribute("cardid").Value.ToBytesFromHex();
            string cardType = cardmng.Attribute("cardtype").Value;
            string update = cardmng.Attribute("update").Value;

            var card = ctx.Cards.SingleOrDefault(c => c.CardId.SequenceEqual(cardId));

            if (card != null)
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("binded", "1"),
                    new XAttribute("dataid", card.DataIdStr),
                    new XAttribute("ecflag", "1"),
                    new XAttribute("newflag", "0"),
                    new XAttribute("expired", "0"),
                    new XAttribute("refid", card.RefIdStr)
                )));
            }
            else
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("status", "112")
                )));
            }

            return data;
        }

        [HttpPost, XrpcCall("cardmng.authpass")]
        public async Task<ActionResult<EamuseXrpcData>> Authpass([FromBody] EamuseXrpcData data)
        {
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            string pass = cardmng.Attribute("pass").Value;
            byte[] refId = cardmng.Attribute("refid").Value.ToBytesFromHex();

            Card card = await ctx.Cards
                .Include(c => c.Player)
                .SingleOrDefaultAsync(c => c.RefId.SequenceEqual(refId));

            int status;
            if (card != null && card.Player != null && card.Player.Passwd == pass)
                status = 0;
            else
                status = 116;

            data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                new XAttribute("status", status)
            )));

            return data;
        }

        [HttpPost, XrpcCall("cardmng.getrefid")]
        public async Task<ActionResult<EamuseXrpcData>> GetRefId([FromBody] EamuseXrpcData data)
        {
            XElement cardmng = data.Document.Element("call").Element("cardmng");

            string cardId = cardmng.Attribute("cardid").Value;
            byte[] cardIdBytes = cardId.ToBytesFromHex();
            string passwd = cardmng.Attribute("passwd").Value;

            if (await ctx.Cards.AnyAsync(c => c.CardId.SequenceEqual(cardIdBytes)))
            {
                data.Document = new XDocument(new XElement("response", new XElement("cardmng")));
                return data;
            }

            Random rng = new Random();

            byte[] dataId = new byte[8];
            byte[] refId = new byte[8];

            rng.NextBytes(dataId);
            rng.NextBytes(refId);

            Player player = new Player()
            {
                Passwd = passwd
            };

            Card card = new Card()
            {
                CardId = cardIdBytes,
                DataId = dataId,
                RefId = refId,
                Player = player
            };

            ctx.Players.Add(player);
            ctx.Cards.Add(card);

            await ctx.SaveChangesAsync();

            data.Document = new XDocument(new XElement("response", new XElement("cardmng",
                    new XAttribute("dataid", card.DataIdStr),
                    new XAttribute("refid", card.RefIdStr)
            )));

            return data;
        }
    }
}
