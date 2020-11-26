using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.FileUpload;
using SynWord_Server_CSharp.DocumentUniqueUp;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.UserData;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase {
        private IWebHostEnvironment _webHostEnvironment;
        private DocxUniqueUp _docxUniqueUp;
        private FileUploadUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;

        private int _dailyLimit = 5;
        private int _fileId = 0;

        public FileUploadController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            _docxUniqueUp = new DocxUniqueUp();
            _usageLog = new FileUploadUsageLog();
        }

        [HttpPost]
        public IActionResult Post([FromForm] FileUploadModel user) {
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++_fileId + "_" + user.Files.FileName;

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx") {
                    return BadRequest("Invalid file extension");
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > _dailyLimit){
                    return BadRequest("dailyLimitReached");
                }

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath)) {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                return new FileStreamResult(stream, mimeType) {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            }
            catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("auth")]
        public IActionResult PostAuth([FromForm] AuthFileUploadModel user)
        {
            try
            {
                _getUserData = new GetUserData(user.uId);
                _setUserData = new SetUserData(user.uId);
                _userDataHandle = new UserDataHandle(user.uId);

                _userDataHandle.CheckUserIdExistIfNotCreate();

                if (_getUserData.is24HoursPassed())
                {
                    _userDataHandle.ResetDefaults();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    return BadRequest("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetDocumentUniqueUpRequests();

                if (requestsLeft <= 0)
                {
                    return BadRequest("dailyLimitReached");
                }

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++_fileId + "_" + user.Files.FileName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _setUserData.SetDocumentUniqueUpRequests(--requestsLeft);

                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
