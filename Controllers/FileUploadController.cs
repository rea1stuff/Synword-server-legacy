using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        public static int FileID { get; set; } = 0;

        public FileUploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadModel objectFile)
        {
            try
            {
                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++FileID + "_" + objectFile.Files.FileName;

                if (objectFile.Files.Length < 0 || Path.GetExtension(objectFile.Files.FileName) != ".docx")
                {
                    return BadRequest("Invalid file extension");
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    objectFile.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                docxUniqueUp.UniqueUp(filePath);

                var mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                var stream = System.IO.File.OpenRead(filePath);

                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = "Synword_" + objectFile.Files.FileName
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
