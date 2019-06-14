using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Formatters;

using eAmuseCore.Compression;
using eAmuseCore.Crypto;
using eAmuseCore.KBinXML;

namespace ClanServer.Formatters
{
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
                byte[] resData;
                if (data.Encoding != null)
                    resData = new KBinXML(data.Document, data.Encoding).Bytes;
                else
                    resData = new KBinXML(data.Document).Bytes;

                string algo = "none";

                var compressed = LZ77.Compress(resData, 32).ToArray();
                if (compressed.Length < resData.Length)
                {
                    resData = compressed;
                    algo = "lz77";
                }
                compressed = null;

                if (data.EamuseInfo != null)
                    RC4.ApplyEAmuseInfo(data.EamuseInfo, resData);

                return (resData, algo);
            });

            if (data.EamuseInfo != null)
            response.Headers.Add("X-Eamuse-Info", data.EamuseInfo);

            response.Headers.Add("X-Compress", compAlgo);

            response.ContentType = "application/octet-stream";
            response.ContentLength = rawData.Length;

            await response.Body.WriteAsync(rawData, 0, rawData.Length);
        }
    }
}
