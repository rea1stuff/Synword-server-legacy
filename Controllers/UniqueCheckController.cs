using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using UniqueCheck;
using Logging;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase
    {
        private static UniqueCheckFromContentWatchAPI uniqueCheckFromAPI = new UniqueCheckFromContentWatchAPI();
        private static UniqueCheckUsageLog usageLog = new UniqueCheckUsageLog();

        private int dailyLimit = 10;
        private int symbolLimit = 20000;

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UniqueCheckModel uniqueCheck)
        {
            try
            {
                var clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                usageLog.CheckIPexistsIfNotThenCreate(clientIP);

                if (symbolLimit < uniqueCheck.text.Length)
                {
                    return BadRequest("symbolLimitReached");
                }

                if (usageLog.getUsesIn24H(clientIP) > dailyLimit)
                {
                    return BadRequest("dailyLimitReached");
                }

                var response = await uniqueCheckFromAPI.postReqest(uniqueCheck.text);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
