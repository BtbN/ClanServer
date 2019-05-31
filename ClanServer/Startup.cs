using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClanServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.InputFormatters.Insert(0, new Formatters.EamuseXrpcInputFormatter());
                    options.OutputFormatters.Insert(0, new Formatters.EamuseXrpcOutputFormatter());

                    options.Conventions.Add(new Routing.XrpcCallConvention());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"[{context.Connection.RemoteIpAddress}] | {context.Request.Path}");
                await next.Invoke();
            });
        }
    }
}
