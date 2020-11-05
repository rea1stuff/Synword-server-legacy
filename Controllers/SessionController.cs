using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Logging;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase {
        private VisitsLog _visitation = new VisitsLog();

        [HttpGet]
        public void Get() {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            _visitation.CheckIpExistsIfNotThenCreate(clientIp);
            _visitation.IncrementNumberOfVisitsIn24Hours(clientIp);
        }
    }
}
