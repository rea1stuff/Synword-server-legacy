using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Exceptions;
using System.Collections.Generic;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.Model.UserData;
using Newtonsoft.Json;
using SynWord_Server_CSharp.DailyCoins;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase {
        IDailyCoinsGet coinsGet;
        
        [HttpPost]
        public IActionResult GetUserData([FromBody] string uId) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
            };
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Start));

                IUserDataHandler<UnauthUserApplicationDataModel> dataHandler = new UnauthUserApplicationDataHandler();

                UnauthUserApplicationDataModel userData = dataHandler.GetUserData(uId);

                userData.lastVisitDate = DateTime.Now.ToString();
                dataHandler.SetUserData(userData);

                //Всегда актуальный ip в связке с токеном
                if (userData.ip != clientIp) {
                    userData.ip = clientIp;
                    dataHandler.SetUserData(userData);
                }

                string response = JsonConvert.SerializeObject(userData);

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
        public IActionResult GetAuthUserData([FromBody] string uId) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "Uid", uId }
            };
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Start));

                coinsGet = new AuthUserDailyCoinsGet(uId);

                if (coinsGet.Is24HoursPassed()) {
                    coinsGet.GiveCoinsToUser();
                }

                UserApplicationDataHandler dataHandler = new UserApplicationDataHandler();

                UserApplicationDataModel userData = dataHandler.GetUserData(uId);

                string response = JsonConvert.SerializeObject(userData);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.Authorization, logInfo, RequestStatuses.Completed));

                return Ok(response);
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.Authorization, logInfo, exception.Message));

                if (new List<Type> { typeof(UserException) }.Contains(exception.GetType())) {
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
