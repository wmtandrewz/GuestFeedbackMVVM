using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class HotelModel
    {
        [JsonProperty("HtlCode")]
        public string HtlCode { get; set; }

        [JsonProperty("HtlName")]
        public string HtlName { get; set; }

        [JsonProperty("TmsId")]
        public string TmsId { get; set; }

        //public HotelModel(string htlCode,string htlName,string tmsId)
        //{
        //    this.HtlCode = htlCode;
        //    this.HtlName = htlName;
        //    this.TmsId = tmsId;
        //}
    }
}
