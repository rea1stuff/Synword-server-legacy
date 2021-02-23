using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.FileUpload;
using SynWord_Server_CSharp.DocumentHandling.Docx;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.UserData;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueCheck;
using SynWord_Server_CSharp.Exceptions;
using Newtonsoft.Json;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Model.Request;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueCheckController : ControllerBase {
        private IWebHostEnvironment _webHostEnvironment;
        private DocxUniqueCheck _docxUniqueCheck;
        private FileUploadUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private DocxGet _docxLimitsCheck;
        private GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        static private int counter = 0;
        private int _fileId;

        public DocxUniqueCheckController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            _usageLog = new FileUploadUsageLog();
            _docxLimitsCheck = new DocxGet();

            counter++;
            _fileId = counter;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", user.AccessToken }
            };

            try {
                RequestLogger.LogRequestStatus(RequestTypes.DocxUniqueCheck, logInfo, RequestStatuses.Start);

                string uId = _googleApi.GetUserId(user.AccessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }

                if (!_getUserData.isPremium()) {
                    throw new Exception("You do not have access to it");
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx") {
                    throw new Exception("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0) {
                    throw new DailyLimitReachedException();
                }

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + _fileId + "_" + "UniqueCheck" + "_" + user.Files.FileName;

                _docxUniqueCheck = new DocxUniqueCheck(filePath);

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

                UniqueCheckResponseModel uniqueCheckResponse = await _docxUniqueCheck.UniqueCheck();

                string response = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                RequestLogger.LogRequestStatus(RequestTypes.DocxUniqueCheck, logInfo, RequestStatuses.Completed);

                return Ok(response);
            } catch (Exception exception) {
                RequestLogger.LogException(RequestTypes.DocxUniqueCheck, logInfo, exception.Message);

                if (new List<Type> { typeof(MaxSymbolLimitReachedException), typeof(DailyLimitReachedException), typeof(UserDoesNotExistException) }.Contains(exception.GetType())) {
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
