using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.UniqueCheck;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase {
        private UniqueCheckFromContentWatchApi _uniqueCheckFromAPI = new UniqueCheckFromContentWatchApi();
        private UniqueCheckUsageLog _usageLog = new UniqueCheckUsageLog();

        private int _dailyLimit = 10;
        private int _symbolLimit = 20000;

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UniqueCheckModel uniqueCheck) {
            try {
                string clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _usageLog.CheckIpExistsIfNotThenCreate(clientIP);

                if (_symbolLimit < uniqueCheck.Text.Length) {
                    return BadRequest("symbolLimitReached");
                }

                if (_usageLog.GetUsesIn24Hours(clientIP) > _dailyLimit) {
                    return BadRequest("dailyLimitReached");
                }

                ActionResult response = await _uniqueCheckFromAPI.PostReqest(uniqueCheck.Text);
                return response;

            }
            catch (Exception exeption) {
                return BadRequest(exeption.Message);
            }
        }
    }
}
