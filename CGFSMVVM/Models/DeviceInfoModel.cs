using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class DeviceInfoModel
    {
        [JsonProperty("DeviceNo")]
        public string DeviceNo { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        [JsonProperty("IsResgistered")]
        public string IsResgistered { get; set; }

       // {"DeviceNo":1,"Notes":"Kasun's iPad","IsResgistered":true}
    }
}
