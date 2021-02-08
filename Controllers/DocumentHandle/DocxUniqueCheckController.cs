﻿using System;
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
        private DocxLimitsCheck _docxLimitsCheck;

        private int _fileId = 0;

        public DocxUniqueCheckController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _usageLog = new FileUploadUsageLog();
            _docxLimitsCheck = new DocxLimitsCheck();
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel user)
        {
            try
            {
                _getUserData = new GetUserData(user.uId);
                _setUserData = new SetUserData(user.uId);
                _userDataHandle = new UserDataHandle(user.uId);
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _userDataHandle.CheckUserIdExistIfNotCreate();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (!_getUserData.isPremium())
                {
                    return BadRequest("You do not have access to it");
                }

                if (_getUserData.is24HoursPassed())
                {
                    _userDataHandle.ResetDefaults();
                }

                if (user.Files.Length < 0 || Path.GetExtension(user.Files.FileName) != ".docx")
                {
                    return BadRequest("Invalid file extension");
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0)
                {
                    return BadRequest("dailyLimitReached");
                }

                string path = _webHostEnvironment.WebRootPath + @"\Uploaded_Files\";
                string filePath = path + ++_fileId + "_" + "UniqueCheck" + "_" + user.Files.FileName;

                _docxUniqueCheck = new DocxUniqueCheck(filePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (_docxLimitsCheck.GetDocSymbolCount(filePath) > _getUserData.GetDocumentMaxSymbolLimit())
                {
                    return BadRequest("document max symbol limit reached");
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    user.Files.CopyTo(fileStream);
                    fileStream.Flush();
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _docxUniqueCheck.UniqueCheck();

                string response = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}