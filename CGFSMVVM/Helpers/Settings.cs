using System.Collections.Generic;
using CGFSMVVM.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace CGFSMVVM.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>

    public static class Settings
    {
        /// <summary>
        /// Gets the app settings.
        /// </summary>
        /// <value>The app settings.</value>
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

		private const string AccessTokenKey = "access_key";
        private static readonly string AccessTokenDefault = string.Empty;

        private const string ExpiresTimeKey = "expires_key";
        private static readonly string ExpiresTimeDefault = string.Empty;

		private const string SAPCookie = "sap-XSRF_GWP_100";
		private static readonly string SettingsSAPCookieDefault = "sap-XSRF_GWP_100";
        
		private const string UsernameKey = "username_key";
        private static readonly string UsernameDefault = string.Empty;

		private const string PasswordKey = "password_key";
        private static readonly string PasswordDefault = string.Empty;

		private const string SAPURL = "SAPURL";
		private static readonly string SettingsSAPURLDefault = "https://alastor.keells.lk:44300";

        /// <summary>
        /// Gets or sets the hotel number.
        /// </summary>
        /// <value>The hotel number.</value>
        public static string HotelIdentifier
        {
            get
            {
				return AppSettings.GetValueOrDefault("HotelIdentifier", string.Empty);
            }
            set
            {
				AppSettings.AddOrUpdateValue("HotelIdentifier", value);
            }
        }

        /// <summary>
        /// Gets or sets the hotel code.
        /// </summary>
        /// <value>The hotel code.</value>
        public static string HotelCode
        {
            get
            {
				return AppSettings.GetValueOrDefault("HotelCode", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("HotelCode", value);
            }
        }
       

        /// <summary>
        /// Gets or sets the SMTPP assword.
        /// </summary>
        /// <value>The SMTPP assword.</value>
        public static string SMTPPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault("SMTPPassword", "hp##2009");
            }
            set
            {
                AppSettings.AddOrUpdateValue("SMTPPassword", value);

            }
        }


        /// <summary>
        /// Gets or sets the base domain URL.
        /// </summary>
        /// <value>The base domain URL.</value>
        public static string BaseDomainURL
        {
            get
            {
                return AppSettings.GetValueOrDefault("BaseDomainURL", "https://cheetah.azure-api.net/api/v1/");
            }
            set
            {
                AppSettings.AddOrUpdateValue("BaseDomainURL", value);

            }
        }

        //public static string SubscriptionKey_Dev
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault("SubscriptionKey_Dev", "d0fbb5e7bebc454e8df6ff295fa73905");
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("SubscriptionKey_Dev", value);
        //    }
        //}

        /// <summary>
        /// Gets or sets the subscription key.
        /// </summary>
        /// <value>The subscription key.</value>
        public static string SubscriptionKey
        {
            get
            {
                return AppSettings.GetValueOrDefault("SubscriptionKey", "c96b7f401241458290ce8544207eb43d");
            }
            set
            {
                AppSettings.AddOrUpdateValue("SubscriptionKey", value);
            }
        }

        /// <summary>
        /// Gets or sets the config APIU ri.
        /// </summary>
        /// <value>The config APIU ri.</value>
        public static string ConfigAPIUri
        {
            get
            {
                return AppSettings.GetValueOrDefault("ConfigAPIUri", "http://chml.keells.lk/FeedbackAPI/api/");
            }
            set
            {
                AppSettings.AddOrUpdateValue("ConfigAPIUri", value);
            }
        }

        /// <summary>
        /// Gets or sets the FTPU ri.
        /// </summary>
        /// <value>The FTPU ri.</value>
        public static string FTPUri
        {
            get
            {
                return AppSettings.GetValueOrDefault("FTPUri", "http://chml.keells.lk/FeedbackAPI/api/");
            }
            set
            {
                AppSettings.AddOrUpdateValue("FTPUri", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:CGFSMVVM.Helpers.Settings"/> email service stat.
        /// </summary>
        /// <value><c>true</c> if email service stat; otherwise, <c>false</c>.</value>
        public static bool EmailServiceStat
        {
            get
            {
                return AppSettings.GetValueOrDefault("EmailServiceStat", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("EmailServiceStat", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:CGFSMVVM.Helpers.Settings"/> AVS tream stat.
        /// </summary>
        /// <value><c>true</c> if AVS tream stat; otherwise, <c>false</c>.</value>
        public static bool AVStreamStat
        {
            get
            {
                return AppSettings.GetValueOrDefault("AVStreamStat", true);
            }
            set
            {
                AppSettings.AddOrUpdateValue("AVStreamStat", value);
            }
        }

        /// <summary>
        /// Gets or sets the app version.
        /// </summary>
        /// <value>The app version.</value>
        public static string AppVersion
        {
            get
            {
                return AppSettings.GetValueOrDefault("AppVersion", "3.5");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AppVersion", value);
            }
        }


        
		public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessTokenKey, value);
            }
        }

        public static string ExpiresTime
        {
            get
            {
                return AppSettings.GetValueOrDefault(ExpiresTimeKey, ExpiresTimeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ExpiresTimeKey, value);
            }
        }
        

        public static string SettingsSAPCookie
        {
            get
            {
                return AppSettings.GetValueOrDefault(SAPCookie, SettingsSAPCookieDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SAPCookie, value);
            }
        }

		public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UsernameKey, value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }

		public static string SettingsSAPURL
        {
            get
            {
                return AppSettings.GetValueOrDefault(SAPURL, SettingsSAPURLDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SAPURL, value);
            }
        }

    }
}