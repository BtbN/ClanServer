using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClanServer
{
    public class Startup
    {
        public const int Port = 9091;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClanServerContext>(options =>
            {
                options.UseSqlite("Data Source=clanserver.db");
            });

            services
                .AddMvc(options =>
                {
                    options.InputFormatters.Insert(0, new Formatters.EamuseXrpcInputFormatter());
                    options.OutputFormatters.Insert(0, new Formatters.EamuseXrpcOutputFormatter());

                    options.Conventions.Add(new Routing.XrpcCallConvention());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[{context.Connection.RemoteIpAddress}] | {context.Request.Path}{context.Request.QueryString}");
                await next.Invoke();
            });

            app.UseMvc();
        }
    }
}
