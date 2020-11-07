using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UniqueCheck {
    public class UniqueCheckResponseModel {
        [JsonProperty("percent")]
        public float Percent { get; set; }
        [JsonProperty("matches")]
        public Match[] Matches { get; set; }
    }
}
