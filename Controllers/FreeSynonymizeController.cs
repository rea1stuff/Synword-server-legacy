using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.Log;
using SynWord_Server_CSharp.RequestProcessor;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FreeSynonymizeController : ControllerBase {
        IUserLogDataModel userLogModel;
        UniqueUpRequestProcessor _requestProcessor = new UniqueUpRequestProcessor();
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UnauthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            userLogModel = new UnauthUserLogDataModel(clientIp, user);

            return await _requestProcessor.UnauthUserRequestExecution(userLogModel);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> PostAuth([FromBody] AuthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            userLogModel = new AuthUserLogDataModel(clientIp, user);

            return await _requestProcessor.AuthUserRequestExecution(userLogModel);
        }
    }
}
