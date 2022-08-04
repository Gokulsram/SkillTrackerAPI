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

                builder.AddConfigurationFromTable(options => options.UseSqlServer(Configuration.GetConnectionString("SkillTrackerDBConnection")));
                Configuration = builder.Build();
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



            services.AddControllers();

            services.AddScoped(typeof(ICacheHelper), typeof(CacheHelper));
            services.AddMemoryCache();
            services.AddTransient(typeof(ICacheProvider), typeof(InMemoryCacheProvider));
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

            services.AddEfCoreRepository();
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

            services.UseCacheWarmer();
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
