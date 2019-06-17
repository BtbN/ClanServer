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
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            var host = builder.Build();

            Console.WriteLine($"Server URL: http://localhost:{Startup.Port}/core");

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] _)
        {
            return new WebHostBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .UseStartup<Startup>()
                .UseKestrel()
                .ConfigureKestrel(ConfigureKestrel);
        }

        private static void ConfigureKestrel(WebHostBuilderContext context, KestrelServerOptions options)
        {
            options.ListenAnyIP(Startup.Port);
        }
    }
}
