using Enyim.Caching.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace SkillTracker.AWSServerless.RabbitMQ.Receive
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddOptions();
                var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
                services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

                var cacheSettings = Configuration.GetSection("CacheSettings");
                services.Configure<CacheConfiguration>(cacheSettings);

                services.AddControllers();

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

                services.AddEnyimMemcached(setup =>
                {
                    setup.Servers.Add(new Server
                    {
                        Address = cacheSettings.GetValue<string>("CacheHost"),
                        Port = cacheSettings.GetValue<int>("CachePort")
                    });
                });

                services.AddDistributedMemoryCache();

                services.AddHttpClient();
                services.AddHostedService<UserSkillUpdateReceiver>();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
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

                app.UseEnyimMemcached();

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
