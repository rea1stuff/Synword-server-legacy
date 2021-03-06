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
using System.Collections.Generic;
using SynWord_Server_CSharp.Model.Request;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueUpController : ControllerBase {
        private DocxUniqueUp _docxUniqueUp;
        private DocxGet _docxLimitsCheck;
        private FileUploadUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        static private int counter = 0;
        private int _fileId;

        public DocxUniqueUpController() {
            _docxUniqueUp = new DocxUniqueUp();
            _usageLog = new FileUploadUsageLog();
            _docxLimitsCheck = new DocxGet();

            counter++;
            _fileId = counter;
        }

        [HttpPost]
        public IActionResult Post([FromForm] FileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, logInfo, RequestStatuses.Start));

                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                string path = ContentRootPath.Path + @"/Files/UploadedFiles/";
                string filePath = path + _fileId + "_" + "UniqueUp" + "_" + user.Files.FileName;

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.UniqueUpRequests) {
                    throw new DailyLimitReachedException();
                }

                using (FileStream fileStream = System.IO.File.Create(filePath)) {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                if (_docxLimitsCheck.GetDocSymbolCount(filePath) > UserLimits.DocumentMaxSymbolLimit) {
                    throw new MaxSymbolLimitReachedException();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx") {
                    throw new Exception("Invalid file extension");
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, logInfo, RequestStatuses.Completed));

                return new FileStreamResult(stream, mimeType) {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.DocxUniqueUp, logInfo, exception.Message));

                if (new List<Type> { typeof(MaxSymbolLimitReachedException), typeof(DailyLimitReachedException) }.Contains(exception.GetType())) {
                    return BadRequest(exception.Message);
                } else {
                    return new ObjectResult(exception.Message) {
                        StatusCode = 500
                    };
                }
            }
        }

        [HttpPost("auth")]
        public IActionResult Authorized([FromForm] AuthFileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", user.AccessToken }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUpAuth, logInfo, RequestStatuses.Start));

                string uId = _googleApi.GetUserId(user.AccessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }
                
                if (user.Files.Length <= 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    throw new Exception("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetUniqueUpRequests();

                if (requestsLeft <= 0) {
                    throw new DailyLimitReachedException();
                }

                string path = ContentRootPath.Path + @"/Files/UploadedFiles/";
                string filePath = path + _fileId + "_" + "AuthUniqueUp" + "_" + user.Files.FileName;

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath)) {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                if (_docxLimitsCheck.GetDocSymbolCount(filePath) > _getUserData.GetDocumentMaxSymbolLimit()) {
                    throw new MaxSymbolLimitReachedException();
                }

                _docxUniqueUp.UniqueUp(filePath);

                string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                FileStream stream = System.IO.File.OpenRead(filePath);

                _setUserData.SetUniqueUpRequest(requestsLeft - 2);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUpAuth, logInfo, RequestStatuses.Completed));

                return new FileStreamResult(stream, mimeType) {
                    FileDownloadName = "Synword_" + user.Files.FileName
                };
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.DocxUniqueUpAuth, logInfo, exception.Message));

                if (new List<Type> { typeof(MaxSymbolLimitReachedException), typeof(DailyLimitReachedException) }.Contains(exception.GetType())) {
                    return BadRequest(exception.Message);
                } else {
                    return new ObjectResult(exception.Message) {
                        StatusCode = 500
                    };
                }
            }
        }
    }
}
