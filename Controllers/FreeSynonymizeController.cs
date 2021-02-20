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

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreeSynonymizeController : ControllerBase
    {
        private ISynonymizer _freeSynonymizer;
        private FreeSynonimizerUsageLog _usageLog;
        private GetUserData _getUserData;
        private SetUserData _setUserData;
        private UserDataHandle _userDataHandle;

        public FreeSynonymizeController()
        {
            _freeSynonymizer = new FreeSynonymizer();
            _usageLog = new FreeSynonimizerUsageLog();
        }

        [HttpPost]
        public IActionResult Post([FromBody] string text)
        {
            Console.WriteLine("Request: UniqueUp");
            try
            {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (text.Length > UserLimits.UniqueUpMaxSymbolLimit)
                {
                    throw new MaxSymbolLimitReachedException();
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > UserLimits.UniqueCheckRequests)
                {
                    throw new DailyLimitReachedException();
                }

                UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(text);
                string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                Console.WriteLine("Request: UniqueUp [COMPLETED]");

                return new ObjectResult(uniqueUpResponseJson)
                {
                    StatusCode = 200
                };
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
        public IActionResult PostAuth([FromBody] AuthUserModel user)
        {
            Console.WriteLine("Request: UniqueUpAuth");
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

                if (user.text.Length > _getUserData.GetUniqueUpMaxSymbolLimit())
                {
                    throw new MaxSymbolLimitReachedException();
                }

                int requestsLeft = _getUserData.GetUniqueUpRequests();

                if (requestsLeft <= 0)
                {
                    throw new DailyLimitReachedException();
                }

                UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(user.text);
                string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

                _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);
                _setUserData.SetUniqueUpRequest(--requestsLeft);

                System.Console.WriteLine("Request: UniqueUpAuth [COMPLETED]");

                return new ObjectResult(uniqueUpResponseJson)
                {
                    StatusCode = 200
                };
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
