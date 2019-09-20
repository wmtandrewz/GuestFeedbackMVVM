using System;
using System.Net.Http;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;

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
                    return "error";
                }
            }

            catch (Exception)
            {
                return "error";
            }

        }


    }
}
