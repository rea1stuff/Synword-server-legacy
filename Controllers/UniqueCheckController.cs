using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Model.UniqueCheck;
using SynWord_Server_CSharp.Exceptions;
using Newtonsoft.Json;
using SynWord_Server_CSharp.GoogleApi;
using System.Collections.Generic;
using SynWord_Server_CSharp.Model.Request;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private UniqueCheckApi _uniqueCheckFromApi;
        private UniqueCheckUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        public UniqueCheckController() {
            _uniqueCheckFromApi = new UniqueCheckApi();
            _usageLog = new UniqueCheckUsageLog();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string text) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "TextLength", text.Length }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueCheck, logInfo, RequestStatuses.Start));

                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (UserLimits.UniqueCheckMaxSymbolLimit < text.Length) {
                    throw new MaxSymbolLimitReachedException();
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.UniqueCheckRequests) {
                    throw new DailyLimitReachedException();
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.UniqueCheck(text);

                string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueCheck, logInfo, RequestStatuses.Completed));

                return new OkObjectResult(uniqueCheckResponseJson);
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueCheck, logInfo, exception.Message));

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
        public async Task<ActionResult> PostAuth([FromBody] AuthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", user.AccessToken },
                { "TextLength", user.Text.Length }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueCheckAuth, logInfo, RequestStatuses.Start));

                string uId = _googleApi.GetUserId(user.AccessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }

                if (user.Text.Length > _getUserData.GetUniqueCheckMaxSymbolLimit()) {
                    throw new MaxSymbolLimitReachedException();
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0) {
                    throw new DailyLimitReachedException();
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.UniqueCheck(user.Text);

                string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueCheckAuth, logInfo, RequestStatuses.Completed));

                return new OkObjectResult(uniqueCheckResponseJson);

            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueCheckAuth, logInfo, exception.Message));

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
