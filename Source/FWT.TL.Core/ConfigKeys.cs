using FWT.TL.Core.Extensions;
using System;
using System.Configuration;

namespace FWT.TL.Core
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

        public static string RsaPublicKey
        {
            get { return "-----BEGIN RSA PUBLIC KEY-----\nMIIBCgKCAQEAwVACPi9w23mF3tBkdZz+zwrzKOaaQdr01vAbU4E1pvkfj4sqDsm6\nlyDONS789sVoD/xCS9Y0hkkC3gtL1tSfTlgCMOOul9lcixlEKzwKENj1Yz/s7daS\nan9tqw3bfUV/nqgbhGX81v/+7RFAEd+RwFnK7a+XYl9sluzHRyVVaTTveB2GazTw\nEfzk2DWgkBluml8OREmvfraX3bkHZJTKX4EQSjBbbdJ2ZXIsRrYOXfaA+xayEGB+\n8hdlLmAjbCVfaigxX0CDqWeR1yFL9kwd9P0NsZRPsmoqVwMbMu7mStFai6aIhc3n\nSlv8kg9qv1m6XHVQY3PnEw+QQtqSIXklHwIDAQAB\n-----END RSA PUBLIC KEY-----"; }
        }

        public static string RedisConnectionString
        {
            get { return Setting("RedisConnectionString"); }
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
