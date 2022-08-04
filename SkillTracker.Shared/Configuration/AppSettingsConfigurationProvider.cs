using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Serilog;
using SkillTracker.Common.Helpers;
using System;
using System.IO;

namespace SkillTracker.Shared
{
    public class AppSettingsConfigurationProvider : JsonConfigurationProvider
    {
        private readonly string _encryptionKey;
        private readonly string _encryptionMarkerStart;
        private readonly string _encryptionMarkerEnd;
        private const string ENCRYPTION_MARKER_START_KEY = "DecryptionSetting:EncryptionMarkerStart";
        public AppSettingsConfigurationProvider(AppSettingsConfigurationSource source, string encryptionKey, string encryptionMarkerStart, string encryptionMarkerEnd) : base(source)
        {
            _encryptionKey = encryptionKey;
            _encryptionMarkerStart = encryptionMarkerStart;
            _encryptionMarkerEnd = encryptionMarkerEnd;
        }
        public override void Load(Stream stream)
        {
            base.Load(stream);
            if (!string.IsNullOrEmpty(_encryptionKey) && !string.IsNullOrEmpty(_encryptionMarkerStart) && !string.IsNullOrEmpty(_encryptionMarkerEnd))
            {
                Log.Information("Call Decryption utility {0}{1}", ENCRYPTION_MARKER_START_KEY, _encryptionKey);
                try
                {
                    Data = ConfigurationProviderHelper.DecryptConfigurationDictionary(Data, _encryptionKey, _encryptionMarkerStart, _encryptionMarkerEnd, new string[] { ENCRYPTION_MARKER_START_KEY });
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Exception Occured in decryption");
                }
            }
        }
    }
    public class AppSettingsConfigurationSource : JsonConfigurationSource
    {
        private readonly string _encryptionKey;
        private readonly string _encryptionMarkerStart;
        private readonly string _encryptionMarkerEnd;
        public AppSettingsConfigurationSource(string encryptionKey, string encryptionMarkerStart, string encryptionMarkerEnd)
        {
            _encryptionKey = encryptionKey;
            _encryptionMarkerStart = encryptionMarkerStart;
            _encryptionMarkerEnd = encryptionMarkerEnd;
        }
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new AppSettingsConfigurationProvider(this, _encryptionKey, _encryptionMarkerStart, _encryptionMarkerEnd);
        }
    }
    public static class JsonConfigurationExtension2
    {
        public static IConfigurationBuilder AddPortalJsonFileProvider(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange,
          string encryptionKey, string encryptionMarkerStart, string encryptionMarkerEnd)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.");
            }
            var source = new AppSettingsConfigurationSource(encryptionKey, encryptionMarkerStart, encryptionMarkerEnd)
            {
                FileProvider = null,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            source.ResolveFileProvider();
            builder.Add(source);
            return builder;
        }
    }
}
