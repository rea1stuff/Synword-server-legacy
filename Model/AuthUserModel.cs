using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    public class AuthUserModel {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
