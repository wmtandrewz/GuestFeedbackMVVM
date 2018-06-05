using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Newtonsoft.Json;

namespace CGFSMVVM.DataParsers
{
    /// <summary>
    /// Configuration deserializer class.
    /// </summary>
    public static class ConfigDeserializer
    {
        /// <summary>
        /// Deserializes the configurations.
        /// </summary>
        /// <returns>The configurations <see cref="T:CGFSMVVM.Models.Configmodel"/></returns>
        public static async Task<ConfigModel> DeserializeConfigurations()
        {
            var responce = await ConfigurationAPIServices.GetSystemConfigs();

            if (responce != null && responce.Contains("ConfigID"))
            {
                return JsonConvert.DeserializeObject<ConfigModel>(responce);
            }
            else
            {
                return null;
            }
        }
    }
}
