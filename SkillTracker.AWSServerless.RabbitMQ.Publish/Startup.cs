using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using SkillTracker.Shared;
using System;

namespace SkillTracker.AWSServerless.RabbitMQ.Publish
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            try
            {
                Configuration = configuration;
                var builder = BuildJsonConfiguration.AddConfigurationBuilder(configuration);
                var logconfig = Configuration.GetSection(nameof(LogSetting));
                Serilogging.CreateLogger(logconfig);

                Configuration = builder.Build();
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddHealthChecks();
                services.AddOptions();
                var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
                services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

                services.AddControllers();

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skill Tracker API", Version = "v1" });
                });

                services.Add(new ServiceDescriptor(typeof(IConfiguration),
                  provider => Configuration,
                  ServiceLifetime.Singleton));

                services.Configure<FormOptions>(o =>
                {
                    o.ValueLengthLimit = int.MaxValue;
                    o.MultipartBodyLengthLimit = int.MaxValue;
                    o.MemoryBufferThreshold = int.MaxValue;
                });

                services.AddControllers().AddNewtonsoftJson(options =>
                  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

                services.AddHttpClient();
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                app.UseRouting();

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Skill Tracker API V1");

                });

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }
    }
}
