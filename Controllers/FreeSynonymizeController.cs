using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synonymize;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FreeSynonymizeController : ControllerBase {
        private ISynonymizer _freeSynonymizer;
        private FreeSynonimizerUsageLog _usageLog;

        public FreeSynonymizeController() {
            _freeSynonymizer = new FreeSynonymizer();
            _usageLog = new FreeSynonimizerUsageLog();
        }

        [HttpPost]
        public IActionResult Post([FromBody] string text) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _usageLog.CheckIpExistsIfNotThenCreate(clientIp);
            
            if (text.Length < 20000) {
                try {
                    string content = _freeSynonymizer.Synonymize(text);

                    _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                    return new ObjectResult(content) {
                        StatusCode = 200
                    };
                } catch {
                    return StatusCode(500);
                }
            } else {
                return BadRequest("Exceeded character limit.");
            }
        }
    }
}
