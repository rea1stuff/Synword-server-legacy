using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.FileUpload;
using SynWord_Server_CSharp.DocumentHandling.Docx;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueUpController : ControllerBase {
        private IWebHostEnvironment _webHostEnvironment;
        private DocxUniqueUp _docxUniqueUp;
        private DocxGet _docxLimitsCheck;
        private FileUploadUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        static private int counter = 0;
        private int _fileId;

        public DocxUniqueUpController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            _docxUniqueUp = new DocxUniqueUp();
            _usageLog = new FileUploadUsageLog();
            _docxLimitsCheck = new DocxGet();

            counter++;
            _fileId = counter;
        }

        [HttpPost]
        public IActionResult Post([FromForm] FileUploadModel user) {
            Console.WriteLine("Request: DocxUniqueUp");
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + _fileId + "_" + "UniqueUp" + "_" + user.Files.FileName;

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.DocumentUniqueUpRequests)
                {
                    throw new DailyLimitReachedException();
                }

                using (FileStream fileStream = System.IO.File.Create(filePath)) {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                if (_docxLimitsCheck.GetDocSymbolCount(filePath) > UserLimits.DocumentMaxSymbolLimit)
                {
                    throw new MaxSymbolLimitReachedException();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    throw new Exception("Invalid file extension");
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                Console.WriteLine("Request: DocxUniqueUp [COMPLETED]");

                return new FileStreamResult(stream, mimeType) {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            }
            catch (MaxSymbolLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (DailyLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return new ObjectResult(exception.Message)
                {
                    StatusCode = 500
                };
            }
        }

        [HttpPost("auth")]
        public IActionResult Authorized([FromForm] AuthFileUploadModel user)
        {
            Console.WriteLine("Request: DocxUniqueUpAuth");
            try
            {
                string uId = _googleApi.GetUserId(user.accessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist())
                {
                    throw new UserDoesNotExistException();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    throw new Exception("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetDocumentUniqueUpRequests();

                if (requestsLeft <= 0)
                {
                    throw new DailyLimitReachedException();
                }

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + _fileId + "_" + "UniqueUp" + "_" + user.Files.FileName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                if (_docxLimitsCheck.GetDocSymbolCount(filePath) > _getUserData.GetDocumentMaxSymbolLimit())
                {
                    throw new MaxSymbolLimitReachedException();
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _setUserData.SetDocumentUniqueUpRequests(--requestsLeft);

                Console.WriteLine("Request: DocxUniqueUpAuth [COMPLETED]");

                return new FileStreamResult(stream, mimeType)
                {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            }
            catch (MaxSymbolLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (DailyLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return new ObjectResult(exception.Message)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
