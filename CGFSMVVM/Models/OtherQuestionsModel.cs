using System;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class OtherQuestionsModel
    {
        [JsonProperty("QOId")]
        public string QOId { get; set; }

        [JsonProperty("QODesc")]
        public string QODesc { get; set; }

    }
}
