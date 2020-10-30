using Microsoft.AspNetCore.Http;

namespace DocumentUniqueUp
{
    public class FileUploadModel
    {
        public IFormFile Files { get; set; }
    }
}
