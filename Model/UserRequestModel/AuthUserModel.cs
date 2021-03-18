using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    public class AuthUserModel : IUserModel {
        [JsonProperty("uId")]
        public string Uid { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
