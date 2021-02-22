using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Exceptions;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase {
        GetUserData _getUserData;
        UserDataHandle _userDataHandle;
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        [HttpPost]
        public ActionResult GetUserData([FromBody] string accessToken) {
            try {
                Console.WriteLine("Request: Authorization");

                string uId = _googleApi.GetUserId(accessToken);
                _userDataHandle = new UserDataHandle(uId);
                _getUserData = new GetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    _userDataHandle.CreateUser();
                }

                string response = _getUserData.GetAllUserData();

                Console.WriteLine("Request: Authorization [COMPLETED]");

                return Ok(response);
            } catch (InvalidTokenException ex) {
                Console.WriteLine("Authorization Exception: " + ex.Message);
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                Console.WriteLine("Authorization Exception: " + ex.Message);
                return new ObjectResult(ex.Message) {
                    StatusCode = 500
                };
            }
        }
    }
}
