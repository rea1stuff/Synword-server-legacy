using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.Log;
using SynWord_Server_CSharp.RequestProcessor;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private IUserLogDataModel userLogModel;
        private UniqueCheckRequestProcessor requestProcessor = new UniqueCheckRequestProcessor();

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UnauthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            userLogModel = new UnauthUserLogDataModel(clientIp, user);

            return await requestProcessor.UnauthUserRequestExecution(userLogModel);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> PostAuth([FromForm] AuthUserModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            userLogModel = new AuthUserLogDataModel(clientIp, user);

            return await requestProcessor.AuthUserRequestExecution(userLogModel);
        }
    }
}
