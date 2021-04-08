using Imageflow.Server;
using Imageflow.Server.HybridCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ImageOptimizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddImageflowHybridCache(new HybridCacheOptions(Configuration.GetSection("ApplicationPaths").GetSection("Cache").Value)
            {
                CacheSizeLimitInBytes = (long)1 * 1024 * 1024 * 1024 //1GB
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseImageflow(new ImageflowMiddlewareOptions()
                .MapPath("/", Configuration.GetSection("ApplicationPaths").GetSection("Xml").Value)
                .SetAllowCaching(true)
                .SetMyOpenSourceProjectUrl("https://github.com/SZRitter/IngeniuxImageOptimizer")
                );
        }
    }
}