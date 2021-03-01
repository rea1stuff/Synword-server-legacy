using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.Model.UniqueUp;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;
using System.Collections.Generic;
using SynWord_Server_CSharp.Model.Request;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FreeSynonymizeController : ControllerBase {
        private ISynonymizer _freeSynonymizer;
        private FreeSynonimizerUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;
        private GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        public FreeSynonymizeController() {
            _freeSynonymizer = new FreeSynonymizer();
            _usageLog = new FreeSynonimizerUsageLog();
        }

        [HttpPost]
        public IActionResult Post([FromBody] string text) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "TextLength", text.Length }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, logInfo, RequestStatuses.Start));

                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (text.Length > UserLimits.UniqueUpMaxSymbolLimit) {
                    throw new MaxSymbolLimitReachedException();
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.UniqueCheckRequests) {
                    throw new DailyLimitReachedException();
                }

                UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(text);
                string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, logInfo, RequestStatuses.Completed));

                return new ObjectResult(uniqueUpResponseJson) {
                    StatusCode = 200
                };
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueUp, logInfo, exception.Message));

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
        public IActionResult PostAuth([FromBody] AuthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", user.AccessToken },
                { "TextLength", user.Text.Length }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUpAuth, logInfo, RequestStatuses.Start));

                string uId = _googleApi.GetUserId(user.AccessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }

                if (user.Text.Length > _getUserData.GetUniqueUpMaxSymbolLimit()) {
                    throw new MaxSymbolLimitReachedException();
                }

                int requestsLeft = _getUserData.GetUniqueUpRequests();

                if (requestsLeft <= 0) {
                    throw new DailyLimitReachedException();
                }

                UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(user.Text);
                string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

                _setUserData.SetUniqueUpRequest(--requestsLeft);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUpAuth, logInfo, RequestStatuses.Completed));

                return new ObjectResult(uniqueUpResponseJson) {
                    StatusCode = 200
                };
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueUpAuth, logInfo, exception.Message));

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
