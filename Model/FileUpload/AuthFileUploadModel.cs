using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.FileUpload {
    public class AuthFileUploadModel {
        [JsonProperty("uId")]
        public string Uid { get; set; }
        public IFormFile Files { get; set; }
    }
}
