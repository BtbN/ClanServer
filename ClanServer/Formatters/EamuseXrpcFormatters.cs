using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

using eAmuseCore.Compression;
using eAmuseCore.Crypto;
using eAmuseCore.KBinXML;
using System.IO;

namespace ClanServer.Formatters
{
    public class EamuseXrpcData
    {
        public KBinXML Data;
        public string EamuseInfo;
    }

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

            if (!context.HttpContext.Request.Headers.ContainsKey("X-Eamuse-Info"))
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

            if (!context.HttpContext.Request.Headers.TryGetValue("X-Eamuse-Info", out var header))
                return await InputFormatterResult.FailureAsync();

            string eAmuseInfo = header.ToString();

            if (!context.HttpContext.Request.Headers.TryGetValue("X-Compress", out header))
                return await InputFormatterResult.FailureAsync();

            string compAlgo = header.ToString();

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
                var decrypted = RC4.ApplyEAmuseInfo(eAmuseInfo, data);
                switch (compAlgo.ToLower())
                {
                    case "lz77":
                        return LZ77.Decompress(decrypted).ToArray();
                    case "none":
                        return decrypted.ToArray();
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
                    return null;
                }
            });

            if (result == null)
                return await InputFormatterResult.FailureAsync();

            return await InputFormatterResult.SuccessAsync(new EamuseXrpcData()
            {
                Data = result,
                EamuseInfo = eAmuseInfo
            });
        }
    }

    public class EamuseXrpcOutputFormatter : OutputFormatter
    {
        public EamuseXrpcOutputFormatter()
        {
            SupportedMediaTypes.Add("application/octet-stream");
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(EamuseXrpcData);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            if (!(context.Object is EamuseXrpcData data))
                throw new ArgumentNullException("Input EamuseXrpcData is null");

            (byte[] rawData, string compAlgo) = await Task.Run(() =>
            {
                byte[] resData = data.Data.Bytes;
                string algo = "none";

                var compressed = LZ77.Compress(resData).ToArray();
                if (compressed.Length < resData.Length)
                {
                    resData = compressed;
                    algo = "lz77";
                }
                compressed = null;

                resData = RC4.ApplyEAmuseInfo(data.EamuseInfo, resData).ToArray();

                return (resData, algo);
            });

            response.Headers.Add("X-Eamuse-Info", data.EamuseInfo);
            response.Headers.Add("X-Compress", compAlgo);
            response.ContentType = "application/octet-stream";
            response.ContentLength = rawData.Length;

            await response.Body.WriteAsync(rawData, 0, rawData.Length);
        }
    }
}
