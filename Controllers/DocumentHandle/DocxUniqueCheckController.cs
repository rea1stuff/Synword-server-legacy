using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.FileUpload;
using SynWord_Server_CSharp.Model.Log.Documents;
using SynWord_Server_CSharp.RequestProcessor;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DocxUniqueCheckController : ControllerBase {
        string path = ContentRootPath.Path + @"/Files/UploadedFiles/";
        DocxUniqueCheckRequestProcessor processor;
        static private int counter = 0;
        private int _fileId;

        public DocxUniqueCheckController() {
            counter++;
            _fileId = counter;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorized([FromForm] AuthFileUploadModel user) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            string filePath = path + _fileId + "_" + "UniqueCheck" + "_" + user.Files.FileName;

            processor = new DocxUniqueCheckRequestProcessor(filePath);

            DocUniqueCheckLogDataModel userLogModel = new DocUniqueCheckLogDataModel(clientIp, user);

            return await processor.AuthUserRequestExecution(userLogModel);
        }
    }
}
