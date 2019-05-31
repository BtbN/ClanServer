using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

using Microsoft.AspNetCore.Mvc.Formatters;

using eAmuseCore.Compression;
using eAmuseCore.Crypto;
using eAmuseCore.KBinXML;
using System.Text;

namespace ClanServer.Formatters
{
    public class EamuseXrpcInputFormatter : InputFormatter
    {
        public EamuseXrpcInputFormatter()
        {
            SupportedMediaTypes.Add("application/octet-stream");
        }

        public override bool CanRead(InputFormatterContext context)
        {
            var contentType = context.HttpContext.Request.ContentType;

            if (!string.IsNullOrEmpty(contentType) && contentType != "application/octet-stream")
                return false;

            if (!context.HttpContext.Request.Headers.TryGetValue("User-Agent", out var ua))
                return false;

            string userAgent = ua.ToString().ToUpper();
            if (userAgent != "EAMUSE.XRPC/1.0")
                return false;

            if (!context.HttpContext.Request.Headers.TryGetValue("X-Compress", out ua))
                return false;

            string compAlgo = ua.ToString().ToLower();

            switch (compAlgo)
            {
                case "lz77":
                case "none":
                    return true;
                default:
                    return false;
            }
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(EamuseXrpcData);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            if (!context.HttpContext.Request.Headers.TryGetValue("X-Compress", out var header))
                return await InputFormatterResult.FailureAsync();

            string compAlgo = header.ToString();

            string eAmuseInfo = null;
            if (context.HttpContext.Request.Headers.TryGetValue("X-Eamuse-Info", out header))
                eAmuseInfo = header.ToString();

            byte[] data;

            using (var ms = new MemoryStream((int)(request.ContentLength ?? 512L)))
            {
                await request.Body.CopyToAsync(ms);
                data = ms.ToArray();
            }

            return await ProcessInputData(data, eAmuseInfo, compAlgo);
        }

        private async Task<InputFormatterResult> ProcessInputData(byte[] data, string eAmuseInfo, string compAlgo)
        {
            data = await Task.Run(() =>
            {
                IEnumerable<byte> rawData = data;
                if (eAmuseInfo != null)
                    rawData = RC4.ApplyEAmuseInfo(eAmuseInfo, data);

                switch (compAlgo.ToLower())
                {
                    case "lz77":
                        return LZ77.Decompress(rawData).ToArray();
                    case "none":
                        return rawData.ToArray();
                    default:
                        return null;
                }
            });

            if (data == null)
                return await InputFormatterResult.FailureAsync();

            KBinXML result = await Task.Run(() =>
            {
                try
                {
                    return new KBinXML(data);
                }
                catch (Exception)
                {
                    Console.WriteLine("Got invalid binary XML input!");
                    return null;
                }
            });

            if (result == null)
                return await InputFormatterResult.FailureAsync();

            return await InputFormatterResult.SuccessAsync(new EamuseXrpcData()
            {
                Document = result.Document,
                Encoding = result.BinEncoding,
                EamuseInfo = eAmuseInfo
            });
        }
    }
}
