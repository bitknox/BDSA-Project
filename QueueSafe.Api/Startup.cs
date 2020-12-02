using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using QueueSafe.Entities;
using QueueSafe.Models;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace QueueSafe.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QueueSafeContext>(options =>
               options.UseNpgsql(Configuration.GetConnectionString("ConnectionString"),
               b => b.MigrationsAssembly("QueueSafe.Api")));
            services.AddDbContext<QueueSafeContext>();
            services.AddScoped<IQueueSafeContext, QueueSafeContext>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddMvc(setupAction =>
            {
                setupAction.EnableEndpointRouting = false;
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0); // Make Json Pascal case

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking API", Version = "v1" });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
