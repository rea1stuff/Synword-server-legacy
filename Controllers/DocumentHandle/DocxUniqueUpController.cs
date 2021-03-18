using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.FileUpload;
using SynWord_Server_CSharp.Model.Log.Documents;
using SynWord_Server_CSharp.RequestProcessor;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueUpController : ControllerBase {
        string path = ContentRootPath.Path + @"/Files/UploadedFiles/";
        static private int counter = 0;
        private int _fileId;
        DocxUniqueUpRequestProcessor processor;
        public DocxUniqueUpController() {
            counter++;
            _fileId = counter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            string filePath = path + _fileId + "_" + "UniqueUp" + "_" + user.Files.FileName;

            UnauthDocUniqueUpLogDataModel userLogModel = new UnauthDocUniqueUpLogDataModel(clientIp, user);

            processor = new DocxUniqueUpRequestProcessor(filePath);

            return await processor.UnauthUserRequestExecution(userLogModel);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            string filePath = path + _fileId + "_" + "AuthUniqueUp" + "_" + user.Files.FileName;

            AuthDocUniqueUpLogDataModel userLogModel = new AuthDocUniqueUpLogDataModel(clientIp, user);

            processor = new DocxUniqueUpRequestProcessor(filePath);

            return await processor.AuthUserRequestExecution(userLogModel);
        }
    }
}
