using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueSafe.Frontend.Data;
using System.Net.Http;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Syncfusion.Blazor;

namespace QueueSafe.Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {
            Uri Api = new Uri("http://localhost:5000");
            services.AddSyncfusionBlazor();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddHttpClient<IBookingRemote, BookingRemote> (client =>
            {
                client.BaseAddress = Api;
            });
            services.AddHttpClient<IStoreRemote, StoreRemote> (client =>
            {
                client.BaseAddress = Api;
            });
            services.AddSingleton(_ => new HttpClient { BaseAddress = Api});
            services.AddBlazoredLocalStorage();
            services.AddSweetAlert2();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configuration["License:qrlicense"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

