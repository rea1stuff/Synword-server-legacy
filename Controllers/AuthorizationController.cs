using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.Exceptions;
using System.Collections.Generic;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;
using SynWord_Server_CSharp.Model.UserData;
using Newtonsoft.Json;
using SynWord_Server_CSharp.DailyCoins;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase {
        private IDailyCoinsGet coinsGet;
        private UnauthUserApplicationDataModel unauthUserData;
        private UserApplicationDataModel authUserData;

        IUnauthUserDao _unauthUserDao = new UnauthUserApplicationDataDao();
        IUserApplicationDataDao _authUserDao = new UserApplicationDataDao();
        Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic>();

        [HttpPost]
        public IActionResult GetUserData([FromForm] string uId) {
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                logInfo.Add("Ip", clientIp);
                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Start));

                unauthUserData = _unauthUserDao.GetUserDataById(uId);

                unauthUserData.lastVisitDate = DateTime.Now.ToString();
                _unauthUserDao.SetUserData(unauthUserData);

                //Всегда актуальный ip в связке с токеном
                if (unauthUserData.ip != clientIp) {
                    unauthUserData.ip = clientIp;
                    _unauthUserDao.SetUserData(unauthUserData);
                }

                string response = JsonConvert.SerializeObject(unauthUserData);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Completed));

                return Ok(response);
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.Authorization, logInfo, exception.Message));

                if (new List<Type> { typeof(UserException) }.Contains(exception.GetType().BaseType)) {
                    return BadRequest(exception.Message);
                } else {
                    return new ObjectResult(exception.Message) {
                        StatusCode = 500
                    };
                }
            }
        }

        [HttpPost("auth")]
        public IActionResult GetAuthUserData([FromForm] string uId) {
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                logInfo.Add("Ip", clientIp);
                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Start));

                authUserData = _authUserDao.GetUserDataById(uId);

                coinsGet = new AuthUserDailyCoinsGet(uId);

                if (coinsGet.Is24HoursPassed()) {
                    coinsGet.GiveCoinsToUser();
                }

                string response = JsonConvert.SerializeObject(authUserData);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Completed));

                return Ok(response);
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.Authorization, logInfo, exception.Message));

                if (new List<Type> { typeof(UserException) }.Contains(exception.GetType().BaseType)) {
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
