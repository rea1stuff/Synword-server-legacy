using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model;
using System;
using System.Net;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RewardedAdVerificationController : ControllerBase {
        UserApplicationDataModel _authUserData;
        IUserApplicationDataDao _authUserDao = new UserApplicationDataDao();

        [HttpGet]
        public IActionResult Get([FromQuery] RewAdVerifRequestModel requestData) {
            Console.WriteLine("RewardedAdVerification [START]");
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                IPHostEntry host = Dns.GetHostEntry(clientIp);

                if (!host.HostName.Contains(".google.com")) {
                    throw new Exception("Unknown host");
                }

                _authUserData = _authUserDao.GetUserDataById(requestData.user_id);

                _authUserData.coins += requestData.reward_amount;

                _authUserDao.SetUserData(_authUserData);

                Console.WriteLine("RewardedAdVerification [END]");
                return Ok();
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
