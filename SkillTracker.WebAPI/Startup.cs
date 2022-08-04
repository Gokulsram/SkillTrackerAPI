using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using SkillTracker.Core;
using SkillTracker.Domain;
using SkillTracker.InfraStructure;
using SkillTracker.RabbitMQ;
using SkillTracker.Shared;
using System;
using System.Reflection;
using Enyim.Caching.Configuration;
using System.Collections.Generic;

namespace SkillTracker.WebAPI
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

                bool.TryParse(Configuration.GetSection("CacheSettings:UseInMemoryCache").Value, out bool useinMemory);

                if (useinMemory)
                {
                    builder.AddConfigurationFromTable(options => options.UseSqlServer(Configuration.GetConnectionString("SkillTrackerDBConnection")));
                    Configuration = builder.Build();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddOptions();
            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            var cacheSettings = Configuration.GetSection("CacheSettings");
            services.Configure<CacheConfiguration>(cacheSettings);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skill Tracker API", Version = "v1" });
            });

            string connectionString = Configuration.GetConnectionString("SkillTrackerDBConnection");

            services.AddDbContext<SkillTrackerDBContext>(options =>
            {
                try
                {
                    options.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
              maxRetryCount: 5,
              maxRetryDelay: TimeSpan.FromSeconds(30),
              errorNumbersToAdd: null);
                    });
                }
                catch (System.Exception ex)
                {
                    Log.Error("Exception occurred in Skill Tracker DB Context", ex);
                }
            });
            services.AddTransient<SkillTrackerDBContext>();

            services.AddEntityServices();
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

            if (cacheSettings.GetValue<bool>("UseInMemoryCache"))
            {
                services.AddEfCoreRepository();
                services.AddMemoryCache();
                services.AddTransient(typeof(ICacheProvider), typeof(InMemoryCacheProvider));
                services.AddScoped(typeof(ICacheHelper), typeof(CacheHelper));
                services.UseCacheWarmer();
            }
            else
            {
                services.AddEnyimMemcached(c => c.Servers = new List<Server> { new Server {
                    Address = cacheSettings.GetValue<string>("CacheHost"),
                    Port = cacheSettings.GetValue<int>("CachePort") 
                } });
                services.AddDistributedMemoryCache();
                services.AddTransient(typeof(ICacheProvider), typeof(MemCacheProvider));
            }

            services.AddHttpClient();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IUserSkillUpdateSender, UserSkillUpdateSender>();
            services.AddCQRSServices();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            }
            else
            {
                app.UseMiddleware<GlobalErrorHandlerMiddleware>();
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
    }
}
