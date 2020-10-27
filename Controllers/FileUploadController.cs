using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DocumentUniqueUp;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public static DocxUniqueUp docxUniqueUp = new DocxUniqueUp();

        public FileUploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromForm] FileUploadModel objectFile)
        {
            string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
            string filePath = path + objectFile.files.FileName;

            try
            {
                if (objectFile.files.Length > 0 && Path.GetExtension(objectFile.files.FileName) == ".docx")
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        objectFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    docxUniqueUp.UniqueUp(filePath);

                    var mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    var stream = System.IO.File.OpenRead(filePath);

                    return new FileStreamResult(stream, mimeType)
                    {
                        FileDownloadName = "Synword_" + objectFile.files.FileName
                    };

                }
                else
                {
                    return BadRequest("Invalid file extension");
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
