using Microsoft.AspNetCore.Http;

namespace SynWord_Server_CSharp.Model {
    public class FileUploadModel
    {
        public IFormFile Files { get; set; }
    }
}
