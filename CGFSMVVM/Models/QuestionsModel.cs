using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CGFSMVVM.Models
{
    public class QuestionsModel
    {
        [JsonProperty("MainCategory")]
        public string MainCategory { get; set; }

        [JsonProperty("CategoryRowId")]
        public string CategoryRowId { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("ParentId")]
        public string ParentId { get; set; }

        [JsonProperty("bc")]
        public string bc { get; set; }

        [JsonProperty("QId")]
        public string QId { get; set; }

        [JsonProperty("QNo")]
        public string QNo { get; set; }

        [JsonProperty("QDesc")]
        public string QDesc { get; set; }

        [JsonProperty("QType")]
        public string QType { get; set; }

        [JsonProperty("PageId")]
        public string PageId { get; set; }

        [JsonProperty("RatingCategory")]
        public string RatingCategory { get; set; }

        [JsonProperty("DependantQNo")]
        public string DependantQNo { get; set; }

        [JsonProperty("DependantQValue")]
        public string DependantQValue { get; set; }

        [JsonProperty("UIControl")]
        public string UIControl { get; set; }

        [JsonProperty("DisplayType")]
        public string DisplayType { get; set; }

        [JsonProperty("Optional")]
        public bool Optional { get; set; }

        [JsonProperty("OtherQuestions")]
        public List<OtherQuestionsModel> OtherQuestions { get; set; }

        [JsonProperty("RatingScale")]
        public List<RatingScaleModel> RatingScale { get; set; }
    }
}
