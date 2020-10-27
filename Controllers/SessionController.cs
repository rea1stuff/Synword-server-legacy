using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Logging;

namespace SynWord_Server_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        private static VisitsLog visitation = new VisitsLog();

        [HttpGet]
        public void Get()
        {
            var clientIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            visitation.CheckIPexistsIfNotThenCreate(clientIP);
            visitation.IncrementNumberOfVisitsIn24H(clientIP);
        }
    }
}
