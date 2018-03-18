using Auth.FWT.Core.Extensions;
using System;
using System.Configuration;

namespace Auth.FWT.Core
{
    public static class ConfigKeys
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["AppContext"].ConnectionString;
            }
        }

        public static string HangfireConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["FWTHangfire"].ConnectionString;
            }
        }

        public static string TelegramApiHash
        {
            get { return Setting("TelegramApiHash"); }
        }

        public static int TelegramApiId
        {
            get { return Setting<int>("TelegramApiId"); }
        }

        private static T Setting<T>(string name) where T : struct
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value.IsNull())
            {
                throw new Exception(string.Format("Could not find setting '{0}',", name));
            }

            return value.To<T>();
        }

        private static string Setting(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value.IsNull())
            {
                throw new Exception(string.Format("Could not find setting '{0}',", name));
            }

            return value;
        }
    }
}