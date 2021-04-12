using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase {
        [HttpGet]
        public IActionResult Registration() {
            Console.WriteLine("Unauth Registration");
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string token;
            try {
                UnauthUserApplicationDataHandler unauthDataHandler = new UnauthUserApplicationDataHandler();

                if (!unauthDataHandler.IsUserExist(clientIp)) {
                    unauthDataHandler.CreateUser(clientIp);
                    token = unauthDataHandler.GetToken();
                } else {
                    token = unauthDataHandler.GetUserDataByIp(clientIp).uId;
                }
                Console.WriteLine("Unauth Registration COMPLETED");
                return Ok(token);
            } catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("auth")]
        public IActionResult AuthRegistration([FromForm] string accessToken) {
            Console.WriteLine("Auth Registration");
            try {
                UserApplicationDataHandler authUser = new UserApplicationDataHandler();
                UserGoogleDataDao googleDataDB = new UserGoogleDataDao();

                var googleModel = GoogleAuthApi.GetGoogleUserModel(accessToken);

                if (!authUser.IsUserExist(googleModel.id)) {
                    authUser.CreateUser(googleModel.id);
                    googleDataDB.Create(googleModel);
                }
                Console.WriteLine("Auth Registration COMPLETED");
                return Ok();
            } catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }
    }
}