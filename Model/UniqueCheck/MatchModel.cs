using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UniqueCheck {
    public class Match {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("percent")]
        public float Percent { get; set; }
    }
}
