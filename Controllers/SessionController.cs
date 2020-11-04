using Microsoft.AspNetCore.Mvc;

using Logging;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        private VisitsLog visitation = new VisitsLog();

        [HttpGet]
        public void Get()
        {
            var clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            visitation.CheckIpExistsIfNotThenCreate(clientIP);
            visitation.IncrementNumberOfVisitsIn24Hours(clientIP);
        }
    }
}
