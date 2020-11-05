using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Logging;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private UniqueCheckFromContentWatchApi _uniqueCheckFromApi = new UniqueCheckFromContentWatchApi();
        private UniqueCheckUsageLog _usageLog = new UniqueCheckUsageLog();

        private int _dailyLimit = 10;
        private int _symbolLimit = 20000;

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string text) {
            try {
                string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIp);

                if (_symbolLimit < text.Length) {
                    return BadRequest("symbolLimitReached");
                }

                if (_usageLog.GetUsesIn24Hours(clientIp) > _dailyLimit) {
                    return BadRequest("dailyLimitReached");
                }

                ActionResult response = await _uniqueCheckFromApi.PostReqest(text);
                return response;

            }
            catch (Exception exeption) {
                return BadRequest(exeption.Message);
            }
        }
    }
}
