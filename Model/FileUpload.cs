using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentUniqueUp
{
    public class FileUploadModel
    {
        public IFormFile files { get; set; }
    }
}
