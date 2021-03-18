using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model {
    public class UnauthUserModel : IUserModel {
        [JsonProperty("uId")]
        public string Uid { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
