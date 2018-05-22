using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class RatingScaleModel
    {
        [JsonProperty("RatingScaleNo")]
        public string RatingScaleNo { get; set; }

        [JsonProperty("RatingScaleText")]
        public string RatingScaleText { get; set; }

    }
}
