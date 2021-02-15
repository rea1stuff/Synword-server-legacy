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
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueCheckController : ControllerBase
    {
        private IWebHostEnvironment _webHostEnvironment;
        private DocxUniqueCheck _docxUniqueCheck;
        private FileUploadUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private DocxGet _docxLimitsCheck;

        static private int _fileId = 0;

        public DocxUniqueCheckController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _usageLog = new FileUploadUsageLog();
            _docxLimitsCheck = new DocxGet();
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel user)
        {
            Console.WriteLine("Request: DocxUniqueCheck");
            try
            {
                _userDataHandle = new UserDataHandle(user.uId);
                _userDataHandle.CheckUserIdExistIfNotCreate();

                _getUserData = new GetUserData(user.uId);
                _setUserData = new SetUserData(user.uId);

                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (!_getUserData.isPremium())
                {
                    throw new Exception("You do not have access to it");
                }

                if (_getUserData.is24HoursPassed())
                {
                    _userDataHandle.ResetDefaults();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    throw new Exception("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0)
                {
                    throw new Exception("dailyLimitReached");
                }

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++_fileId + "_" + "UniqueCheck" + "_" + user.Files.FileName;

                _docxUniqueCheck = new DocxUniqueCheck(filePath);

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
                    throw new Exception("document max symbol limit reached");
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _docxUniqueCheck.UniqueCheck();

                string response = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                Console.WriteLine("Request: DocxUniqueCheck [COMPLETED]");

                return Ok(response);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
