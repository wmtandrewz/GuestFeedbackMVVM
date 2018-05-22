using System;
using System.Net.Http;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Configuration API Services.
    /// </summary>
    public static class ConfigurationAPIServices
    {
        /// <summary>
        /// Gets the system configs from API.
        /// </summary>
        /// <returns>The system configs. <see cref="T:CGFSMVVM.Models.ConfigModel"/></returns>
        public static async Task<string> GetSystemConfigs()
        {

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.ConfigAPIUri)
                };
				var response = await client.GetAsync("GFBConfig/GetSystemConfiguration").ConfigureAwait(true);
                var resultVersion = response.Content.ReadAsStringAsync().Result;

				if (!string.IsNullOrEmpty(resultVersion))
                {
                    return resultVersion;
                }
                else
                {
					Analytics.TrackEvent("ConfigurationAPIServices.GetSystemConfigs return is null");
                    return "error";
                }
            }

            catch (Exception exception)
            {
				Analytics.TrackEvent($"ConfigurationAPIServices.GetSystemConfigs return is exception with {exception.Message}");
                Crashes.TrackError(exception);
                return "error";
            }

        }

        /// <summary>
        /// Gets the device information from API.
        /// </summary>
        /// <returns>The device information.</returns>
        /// <param name="uuid">UUID.</param>
        public static async Task<string> GetDeviceInformation(string uuid)
        {

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.ConfigAPIUri)
                };
				var response = await client.GetAsync($"GFBConfig/GetDeviceInfo/{uuid}").ConfigureAwait(true);
                var responceDevice = response.Content.ReadAsStringAsync().Result;

				if (!string.IsNullOrEmpty(responceDevice))
                {
                    return responceDevice;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return null;
            }

        }

        /// <summary>
        /// Registers the device in Database.
        /// </summary>
        /// <returns>The device.</returns>
        /// <param name="uuid">UUID.</param>
        public static async Task<string> RegisterDevice(string uuid)
        {

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.ConfigAPIUri)
                };
                var response = await client.GetAsync($"GFBConfig/RegisterDevice/{uuid}");
                var resultVersion = response.Content.ReadAsStringAsync().Result;

				if (!string.IsNullOrEmpty(resultVersion))
                {
                    return resultVersion;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return null;
            }

        }
    }
}
