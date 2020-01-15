using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetR.Web.Infrastructure;
using NetR.Web.Notification;
using RestSharp;
using Serilog;

namespace NetR.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
            services.AddControllers();
            services.AddDbContextPool<BionoContext>(options => {
                options.UseSqlite("Data Source=bionowatch.db");
            });
            services.AddMemoryCache();
            services.AddSingleton<INotifier, SMSNotifier>();
            services.AddSingleton<INotifier, EmailNotifier>();
            services.AddOptions();
            services.Configure<SmSSettings>(configuration.GetSection("SmSSettings"));
            services.AddSingleton(config => {
                return new RestClient("https://rest.smsportal.com");
            });
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NetRHub>("/hub");
                endpoints.MapDefaultControllerRoute();
            });
           
        }
    }
}
