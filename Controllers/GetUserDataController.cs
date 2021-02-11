using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SynWord_Server_CSharp.UserData;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUserDataController : ControllerBase
    {
        GetUserData getUserData;
        private UserDataHandle _userDataHandle;

        [HttpPost]
        public ActionResult GetUserData([FromBody] string userId)
        {
            try
            {
                _userDataHandle = new UserDataHandle(userId);
                _userDataHandle.CheckUserIdExistIfNotCreate();

                getUserData = new GetUserData(userId);

                string response = getUserData.GetAllUserData();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
