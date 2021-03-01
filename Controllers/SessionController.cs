using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase {
        private VisitsLog _visitation = new VisitsLog();

        [HttpGet]
        public void Get() {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp }
            };

            RequestLogger.Add(new RequestStatusLog(RequestTypes.Session, logInfo, RequestStatuses.Start));

            _visitation.CheckIpExistsIfNotThenCreate(clientIp);
            _visitation.IncrementNumberOfVisitsIn24Hours(clientIp);

            RequestLogger.Add(new RequestStatusLog(RequestTypes.Session, logInfo, RequestStatuses.Completed));
        }
    }
}
