using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Model.UniqueCheck;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private UniqueCheckFromContentWatchApi _uniqueCheckFromApi;
        private UniqueCheckUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;

        public UniqueCheckController()
        {
            _uniqueCheckFromApi = new UniqueCheckFromContentWatchApi();
            _usageLog = new UniqueCheckUsageLog();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string text) {
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (UserLimits.UniqueCheckMaxSymbolLimit < text.Length) {
                    return BadRequest("symbolLimitReached");
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.UniqueCheckRequests) {
                    return BadRequest("dailyLimitReached");
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.PostReqest(text);

                string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                return new OkObjectResult(uniqueCheckResponseJson);
            }
            catch (Exception exeption) {
                return BadRequest(exeption.Message);
            }
        }

        [HttpPost("auth")]
        public async Task<ActionResult> PostAuth([FromBody] AuthUserModel user)
        {
            try
            {
                _userDataHandle = new UserDataHandle(user.uId);
                _userDataHandle.CheckUserIdExistIfNotCreate();

                _getUserData = new GetUserData(user.uId);
                _setUserData = new SetUserData(user.uId);
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (_getUserData.is24HoursPassed())
                {
                    _userDataHandle.ResetDefaults();
                }

                if (user.text.Length > _getUserData.GetUniqueCheckMaxSymbolLimit())
                {
                    return BadRequest("symbolLimitReached");
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0)
                {
                    return BadRequest("dailyLimitReached");
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.PostReqest(user.text);

                string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                return new OkObjectResult(uniqueCheckResponseJson);

            }
            catch (Exception exeption)
            {
                return BadRequest(exeption.Message);
            }
        }
    }
}
