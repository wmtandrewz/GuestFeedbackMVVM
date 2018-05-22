using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    /// <summary>
    /// Config model.
    /// </summary>
    public class ConfigModel
    {
        [JsonProperty("ConfigID")]
        public string ConfigID { get; set; }

        [JsonProperty("AppVersion")]
        public string AppVersion { get; set; }

        [JsonProperty("PriorVersionExpiryDate")]
        public string PriorVersionExpiryDate { get; set; }

        [JsonProperty("IsMultiLingual")]
        public string IsMultiLingual { get; set; }

        [JsonProperty("SendErrorByEmail")]
        public string SendErrorByEmail { get; set; }

        [JsonProperty("SendErrorBySMS")]
        public string SendErrorBySMS { get; set; }

        [JsonProperty("TakePhoto")]
        public string TakePhoto { get; set; }

        [JsonProperty("FTPLocationURI")]
        public string FTPLocationURI { get; set; }

        [JsonProperty("APIM_BaseURI")]
        public string APIM_BaseURI { get; set; }

        [JsonProperty("APIMSubKey")]
        public string APIMSubKey { get; set; }

    }
}
