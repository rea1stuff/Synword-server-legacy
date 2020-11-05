using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.DocumentUniqueUp;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase {
        private IWebHostEnvironment _webHostEnvironment;
        private DocxUniqueUp _docxUniqueUp = new DocxUniqueUp();

        private int _fileId = 0;

        public FileUploadController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult Post([FromForm] FileUploadModel objectFile) {
            try {
                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++_fileId + "_" + objectFile.Files.FileName;

                if (objectFile.Files.Length < 0 || Path.GetExtension(objectFile.Files.FileName) != ".docx") {
                    return BadRequest("Invalid file extension");
                }

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath)) {
                    objectFile.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                return new FileStreamResult(stream, mimeType) {
                    FileDownloadName = "Synword_" + objectFile.Files.FileName
                };
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }
    }
}
