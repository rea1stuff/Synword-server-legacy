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
        private DocxUniqueUpRequestProcessor processor;

        public DocxUniqueUpController() {
            counter++;
            _fileId = counter;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] FileUploadModel requestData) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            string filePath = path + _fileId + "_" + "UniqueUp" + "_" + requestData.Files.FileName;

            UnauthDocUniqueUpLogDataModel userLogModel = new UnauthDocUniqueUpLogDataModel(clientIp, requestData);

            processor = new DocxUniqueUpRequestProcessor(filePath);

            return await processor.UnauthUserRequestExecution(userLogModel);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel requestData) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            string filePath = path + _fileId + "_" + "AuthUniqueUp" + "_" + requestData.Files.FileName;

            AuthDocUniqueUpLogDataModel userLogModel = new AuthDocUniqueUpLogDataModel(clientIp, requestData);

            processor = new DocxUniqueUpRequestProcessor(filePath);

            return await processor.AuthUserRequestExecution(userLogModel);
        }
    }
}
