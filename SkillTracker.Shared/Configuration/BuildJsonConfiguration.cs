using Microsoft.Extensions.Configuration;
using SkillTracker.Common.Helpers;
using System;
using System.IO;

namespace SkillTracker.Shared
{
    public static class BuildJsonConfiguration
    {
        public static ConfigurationBuilder AddConfigurationBuilder(IConfiguration configuration)
        {
            IConfiguration Configuration;
            var builder = new ConfigurationBuilder();
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appsettingFilePath = (environment == "Development") ? Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.json") : $"appsettings.json";
            if (environment == "Development")
            {
                builder.AddJsonFile(appsettingFilePath, optional: true);
            }
            else
            {
                builder.AddJsonFile($"appsettings.json", optional: false);
            }
            Configuration = builder.Build();
            var decryptSetting = Configuration.GetSection(nameof(DecryptionSetting));
            var seccretKey = "SkillTracker_Dev"; //Environment.GetEnvironmentVariable(decryptSetting[nameof(DecryptionSetting.DecrtyptionEnviornmentVaribale)]);
            var encryptionMarkerStart = decryptSetting[nameof(DecryptionSetting.EncryptionMarkerStart)];
            var encryptionMarkerEnd = decryptSetting[nameof(DecryptionSetting.EncryptionMarkerEnd)];
            if (!string.IsNullOrEmpty(seccretKey))
            {
                builder = new ConfigurationBuilder();
                builder.AddPortalJsonFileProvider(appsettingFilePath, optional: true, reloadOnChange: true, encryptionKey: seccretKey, encryptionMarkerStart: encryptionMarkerStart, encryptionMarkerEnd: encryptionMarkerEnd);
            }
            return builder;
        }
    }
}
