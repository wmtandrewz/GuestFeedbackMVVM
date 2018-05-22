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

        /// <summary>
        /// Gets or sets the hotel number.
        /// </summary>
        /// <value>The hotel number.</value>
        public static string HotelNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault("HotelNumber", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("HotelNumber", value);
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
        /// Gets or sets the name of the hotel.
        /// </summary>
        /// <value>The name of the hotel.</value>
        public static string HotelName
        {
            get
            {
                return AppSettings.GetValueOrDefault("HotelName", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("HotelName", value);
            }
        }

        //public static string DevelopmentUrl
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault("DevelopmentUrl", "https://jkhapimdev.azure-api.net/api/beta/v1/");
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("DevelopmentUrl", value);

        //    }
        //}

        //public static string DevelopmentInhouseDate
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault("DevelopmentInhouseDate", "2017-03-18");
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("DevelopmentInhouseDate", value);

        //    }
        //}


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
        /// Gets or sets the hotel list.
        /// </summary>
        /// <value>The hotel list.</value>
        public static string HotelList
        {
            get
            {
                return AppSettings.GetValueOrDefault("HotelList", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("HotelList", value);
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
                return AppSettings.GetValueOrDefault("AppVersion", "3.0.0046");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AppVersion", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public static string DeviceName
        {
            get
            {
                return AppSettings.GetValueOrDefault("DeviceName", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("DeviceName", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:CGFSMVVM.Helpers.Settings"/> is device registered.
        /// </summary>
        /// <value><c>true</c> if is device registered; otherwise, <c>false</c>.</value>
        public static bool IsDeviceRegistered
        {
            get
            {
                return AppSettings.GetValueOrDefault("IsDeviceRegistered", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("IsDeviceRegistered", value);
            }
        }

        /// <summary>
        /// Gets or sets the device current UUID.
        /// </summary>
        /// <value>The device current UUID.</value>
        public static string DeviceCurrentUUID
        {
            get
            {
				return AppSettings.GetValueOrDefault("DeviceCurrentUUID", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("DeviceCurrentUUID", value);
            }
        }

    }
}