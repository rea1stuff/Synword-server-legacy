using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    public class UnauthUserRequestModel : IUserRequestModel {
        [JsonProperty("uId")]
        public string Uid { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
