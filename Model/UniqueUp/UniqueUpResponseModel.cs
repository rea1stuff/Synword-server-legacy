using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UniqueUp {
    public class UniqueUpResponseModel {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("replaced")]
        public ReplacedWordModel[] Replaced { get; set; }
        [JsonProperty("replacedCount")]
        public int ReplacedCount { get; set; }
    }
}
