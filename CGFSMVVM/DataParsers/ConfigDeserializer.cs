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
        public static async Task <ConfigModel> DeserializeConfigurations()
        {
            var responce = await ConfigurationAPIServices.GetSystemConfigs();

            if(responce != null)
            {
                return JsonConvert.DeserializeObject <ConfigModel> (responce);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deserializes the hotels.
        /// </summary>
        /// <returns>The hotels.</returns>
        /// <param name="responce">JSON Responce of ConfigurationAPIServices.RegisterDevice(uuid)</param>
        public static List<HotelModel> DeserializeHotels(string responce)
        {
            List<HotelModel> hotelList = new List<HotelModel>();

            if (responce != null)
            {
                hotelList = JsonConvert.DeserializeObject<List<HotelModel>>(responce);

                if (hotelList != null)
                {
                    return hotelList;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Deserializes the hardware device info.
        /// </summary>
        /// <returns>The device info.</returns>
        /// <param name="responce">Responce of ConfigDeserializer.DeserializeDeviceInfo(args)</param>
        public static DeviceInfoModel DeserializeDeviceInfo(string responce)
        {

            if (responce != null)
            {
                DeviceInfoModel deviceInfoModel = JsonConvert.DeserializeObject<DeviceInfoModel>(responce);

                if (deviceInfoModel != null)
                {
                    return deviceInfoModel;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
