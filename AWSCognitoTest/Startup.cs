using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.SecurityToken.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace AWSCognitoTest
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
            //var credentials = new Credentials
            //{
            //    AccessKeyId = "AKIA4ZH6BSWGZWNRYIZD",
            //    SecretAccessKey = "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"
            //};

            //var provider = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.USEast1);
            //var pool = new CognitoUserPool("ap-south-1_IcIWkqxRU",
            //                                       "4ii01vcbrgq2piqs8mehc43p0v",
            //                                         provider,
            //                                         "de84rqcgf5q1nmj79gg9eko6pj34qdoiul4pbjd70is70bkqc3k");

            //services.AddSingleton<IAmazonCognitoIdentityProvider>(provider);
            //services.AddSingleton(pool);


            services.AddAuthentication("Bearer")
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
                        options.Audience = "4ii01vcbrgq2piqs8mehc43p0v";
                        options.Authority = "https://cognito-idp.ap-south-1.amazonaws.com/ap-south-1_IcIWkqxRU";
                        options.RequireHttpsMetadata = false;
                    });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
            //            {
            //                // get JsonWebKeySet from AWS
            //                var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
            //                // serialize the result
            //                var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
            //                // cast the result to be the type expected by IssuerSigningKeyResolver
            //                return (IEnumerable<SecurityKey>)keys;
            //            },

            //            ValidIssuer = "https://cognito-idp.ap-south-1.amazonaws.com/ap-south-1_IcIWkqxRU",
            //            ValidateIssuerSigningKey = true,
            //            ValidateIssuer = true,
            //            ValidateLifetime = true,
            //            ValidAudience = "4ii01vcbrgq2piqs8mehc43p0v",
            //            ValidateAudience = true
            //        };
            //    });
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
