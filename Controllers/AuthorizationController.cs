using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Exceptions;
using System.Collections.Generic;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase {
        GetUserData _getUserData;
        UserDataHandle _userDataHandle;
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        [HttpPost]
        public ActionResult GetUserData([FromBody] string accessToken) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", accessToken }
            };

            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Start));

                string uId = _googleApi.GetUserId(accessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    _userDataHandle.CreateUser();
                }

                string response = _getUserData.GetAllUserData();

                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Completed));

                return Ok(response);
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.Authorization, logInfo, exception.Message));

                if (new List<Type> { typeof(InvalidAccessTokenException) }.Contains(exception.GetType())) {
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
