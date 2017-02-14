using System;
using System.Collections.Generic;
using System.Configuration;

namespace HelloInversion.Code.Application
{
    public class ConfigurationHelper
    {
        public static IDictionary<string, string> GetConfiguration()
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();

            IDictionary<string, string> settingsFromConfig = GetConfigurationFromConfig();

            foreach (KeyValuePair<string, string> kvp in settingsFromConfig)
            {
                try
                {
                    settings.Add(kvp);
                }
                catch (ArgumentException ax)
                {
                    throw new ArgumentException(
                        String.Format("Problem while adding {0}:{1} to settings.", kvp.Key, kvp.Value), ax);
                }
            }

            return settings;
        }

        public static IDictionary<string, string> GetConfigurationFromConfig()
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                settings.Add(key, ConfigurationManager.AppSettings[key]);
            }
            return settings;
        }
    }
}