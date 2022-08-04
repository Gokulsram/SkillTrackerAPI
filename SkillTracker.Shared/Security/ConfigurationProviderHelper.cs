using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkillTracker.Common.Helpers
{
    public static class ConfigurationProviderHelper
    {
        public static IDictionary<string, string> DecryptConfigurationDictionary(IDictionary<string, string> keyValuePairs, string encryptionKey,
          string encryptionMarkerStart, string encryptionMarkerEnd, string[] ignoreKeys)
        {
            if (!string.IsNullOrEmpty(encryptionKey) && !string.IsNullOrEmpty(encryptionMarkerStart) && !string.IsNullOrEmpty(encryptionMarkerEnd))
            {
                var encryptedItems = keyValuePairs.Where(x => x.Value.Contains(encryptionMarkerStart)).Select(item => item.Key).ToList();
                foreach (string encryptedItemKey in encryptedItems)
                {
                    if (!ignoreKeys.Contains(encryptedItemKey))
                    {
                        keyValuePairs[encryptedItemKey] = DecryptConfigurationValueString(keyValuePairs, encryptedItemKey, encryptionKey, encryptionMarkerStart, encryptionMarkerEnd);
                    }
                }
            }
            return keyValuePairs;
        }

        private static string DecryptConfigurationValueString(IDictionary<string, string> keyValuePairs, string encryptedConfigKey, string encryptionKey,
          string encryptionMarkerStart, string encryptionMarkerEnd)
        {
            var ConfigItem = string.Empty;
            try
            {
                ConfigItem = keyValuePairs[encryptedConfigKey];
                while (ConfigItem.Contains(encryptionMarkerStart))
                {
                    var startIndex = ConfigItem.IndexOf(encryptionMarkerStart);
                    var endIndex = ConfigItem.IndexOf(encryptionMarkerEnd);
                    var encryptionSegment = ConfigItem.Substring(startIndex, (endIndex - startIndex) + 1);

                    var stringToDecrypt = encryptionSegment.Remove(0, encryptionMarkerStart.Length);
                    stringToDecrypt = stringToDecrypt.Remove(stringToDecrypt.Length - 1, encryptionMarkerEnd.Length);

                    ConfigItem = ConfigItem.Replace(encryptionSegment, CipherUtility.DecryptString(stringToDecrypt, encryptionKey));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format("Error in Decrypting config Key:{0} Error:{1}", encryptedConfigKey, ex.Message));
            }
            return ConfigItem;
        }
        public static string DecryptString(string encryptedValue, string encryptionKey, string encryptionMarkerStart, string encryptionMarkerEnd)
        {
            try
            {
                var startIndex = encryptedValue.IndexOf(encryptionMarkerStart);
                var endIndex = encryptedValue.IndexOf(encryptionMarkerEnd);
                var encryptionSegment = encryptedValue.Substring(startIndex, (endIndex - startIndex) + 1);

                var stringToDecrypt = encryptionSegment.Remove(0, encryptionMarkerStart.Length);
                stringToDecrypt = stringToDecrypt.Remove(stringToDecrypt.Length - 1, encryptionMarkerEnd.Length);

                encryptedValue = encryptedValue.Replace(encryptionSegment, CipherUtility.DecryptString(stringToDecrypt, encryptionKey));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format("Error in Decrypting config Key:{0} Error:{1}", encryptionKey, ex.Message));
            }
            return encryptedValue;
        }
    }
}
