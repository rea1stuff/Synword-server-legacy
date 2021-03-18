using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase {
        [HttpPost]
        public IActionResult Registration() {
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

                return Ok(token);
            } catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }
        [HttpPost("auth")]
        public IActionResult AuthRegistration([FromBody] string accessToken) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            try {
                IUserDataHandler<UserApplicationDataModel> dataHandler = new UserApplicationDataHandler();
                IUserDataHandler<UserGoogleDataModel> googleDataHandler = new UserGoogleDataHandler();

                string uId = GoogleAuthApi.GetUserId(accessToken);

                if (!dataHandler.IsUserExist(accessToken)) {
                    dataHandler.CreateUser(accessToken);
                    googleDataHandler.CreateUser(accessToken);
                }

                return Ok();
            } catch (Exception exception) {
                return BadRequest(exception.Message);
            }
        }
    }
}