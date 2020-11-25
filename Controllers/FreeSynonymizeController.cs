using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.Model.UniqueUp;
using Newtonsoft.Json;

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
                    UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(text);
                    string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

                    _usageLog.IncrementNumberOfUsesIn24Hours(clientIp);

                    return new ObjectResult(uniqueUpResponseJson) {
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
