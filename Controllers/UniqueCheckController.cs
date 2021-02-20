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

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private UniqueCheckApi _uniqueCheckFromApi;
        private UniqueCheckUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;

        public UniqueCheckController()
        {
            _uniqueCheckFromApi = new UniqueCheckApi();
            _usageLog = new UniqueCheckUsageLog();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string text) {
            Console.WriteLine("Request: UniqueCheck");
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
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

                Console.WriteLine("Request: UniqueCheck [COMPLETED]");

                return new OkObjectResult(uniqueCheckResponseJson);
            }
            catch (MaxSymbolLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (DailyLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("auth")]
        public async Task<ActionResult> PostAuth([FromBody] AuthUserModel user)
        {
            Console.WriteLine("Request: UniqueCheckAuth");
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
                    throw new MaxSymbolLimitReachedException();
                }

                int requestsLeft = _getUserData.GetUniqueCheckRequests();

                if (requestsLeft <= 0)
                {
                    throw new DailyLimitReachedException();
                }

                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.UniqueCheck(user.text);

                string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

                _setUserData.SetUniqueCheckRequest(--requestsLeft);

                Console.WriteLine("Request: UniqueCheckAuth [COMPLETED]");

                return new OkObjectResult(uniqueCheckResponseJson);

            }
            catch (MaxSymbolLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (DailyLimitReachedException exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
