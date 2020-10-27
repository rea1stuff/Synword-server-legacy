using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc;

using UniqueCheck;
using Logging;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniqueCheckController : ControllerBase
    {
        private static readonly ContentWatchAPI_Model contentWatchModel = new ContentWatchAPI_Model();
        private static UniqueCheckFromContentWatchAPI uniqueCheckFromAPI = new UniqueCheckFromContentWatchAPI();
        private static UniqueCheckUsageLog usageLog = new UniqueCheckUsageLog();
        private int dailyLimit = 10;

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UniqueCheckModel uniqueCheck)
        {
            try
            {
                var clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                usageLog.CheckIPexistsIfNotThenCreate(clientIP);

                if (usageLog.getUsesIn24H(clientIP) < dailyLimit)
                {
                    var response = await uniqueCheckFromAPI.postReqest(uniqueCheck.text);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("dailyLimitReached");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
