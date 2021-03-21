using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueCheck;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class UniqueCheckRequestHandler : IRequestHandler {
        private UniqueCheckApi _uniqueCheckFromApi = new UniqueCheckApi();

        public override async Task<IActionResult> HandleRequest(string text) {
            UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheckFromApi.UniqueCheck(text);

            string uniqueCheckResponseJson = JsonConvert.SerializeObject(uniqueCheckResponse);

            return new OkObjectResult(uniqueCheckResponseJson);
        }
    }
}
