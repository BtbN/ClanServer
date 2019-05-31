using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClanServer
{
    public class Program
    {
        public static void Main()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureKestrel(ConfigureKestrel);

            var host = builder.Build();

            host.Run();
        }

        private static void ConfigureKestrel(WebHostBuilderContext context, KestrelServerOptions options)
        {
            options.ListenLocalhost(Startup.Port);
        }
    }
}
