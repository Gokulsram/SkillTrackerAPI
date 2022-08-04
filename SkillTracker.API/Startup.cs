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
using SkillTracker.InfraStructure;
using SkillTracker.Shared;
using System;

namespace SkillTracker.API
{
    public class Startup
    {
        public IConfiguration Configurations { get; set; }
        public Startup(IConfiguration configuration)
        {
            try
            {
                Configurations = configuration;
                var builder = BuildJsonConfiguration.AddConfigurationBuilder(configuration);
                var logconfig = Configurations.GetSection(nameof(LogSetting));
                Serilogging.CreateLogger(logconfig);

                Configurations = builder.Build();

                // Use decrypted connection string
                //var decryptSetting = Configurations.GetSection(nameof(DecryptionSetting));
                ////var secretKey = "SkillTracker_Dev"; //Environment.GetEnvironmentVariable(decryptSetting[nameof(DecryptionSetting.DecrtyptionEnviornmentVaribale)]);
                //var encryptionMarkerStart = decryptSetting[nameof(DecryptionSetting.EncryptionMarkerStart)];
                //var encryptionMarkerEnd = decryptSetting[nameof(DecryptionSetting.EncryptionMarkerEnd)];
                builder.AddConfigurationFromTable(options => options.UseSqlServer(Configurations.GetConnectionString("SkillTrackerDBConnection")));
                Configurations = builder.Build();
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped(typeof(ICacheHelper), typeof(CacheHelper));
            services.AddMemoryCache();
            services.AddTransient(typeof(ICacheProvider), typeof(InMemoryCacheProvider));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skill Tracker API", Version = "v1" });
            });

            string connectionString = Configurations.GetConnectionString("SkillTrackerDBConnection");

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
              provider => Configurations,
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
            //services.AddScoped((serviceProvider) =>
            //{
            //    var config = serviceProvider.GetRequiredService<IConfiguration>();
            //    return new SmtpClient()
            //    {
            //        Host = config.GetValue<string>("Email:SmtpHost"),
            //        Port = config.GetValue<int>("Email:SmtpPort"),
            //        Credentials = new NetworkCredential(config.GetValue<string>("Email:SmtpUserName"), config.GetValue<string>("Email:SmtpPassword")),
            //        EnableSsl = true,
            //    };
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
