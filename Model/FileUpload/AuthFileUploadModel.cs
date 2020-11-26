using Microsoft.AspNetCore.Http;

namespace SynWord_Server_CSharp.Model.FileUpload
{
    public class AuthFileUploadModel
    {
        public string uId { get; set; }
        public IFormFile Files { get; set; }
    }
}
