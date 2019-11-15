using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ARngryBirdsBackend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddCors(options =>
            {
                options.AddPolicy("_myCors",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseFileServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseCors("_myCors");
        }
    }
}