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
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ClanServer
{
    public class Startup
    {
        public const int Port = 9091;

#pragma warning disable CS0618
        public static readonly LoggerFactory LogFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
#pragma warning restore CS0618

        private readonly IHostingEnvironment CurEnv;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            CurEnv = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services
                .AddDbContext<ClanServerContext>(options =>
                {
                    if (CurEnv.IsDevelopment())
                        options.UseLoggerFactory(LogFactory);

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
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ClanServerContext>();
                context.Database.EnsureCreated();
            }

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[{context.Connection.RemoteIpAddress}] | {context.Request.Path}{context.Request.QueryString}");
                await next.Invoke();
            });

            app.UseMvc();
        }
    }
}
