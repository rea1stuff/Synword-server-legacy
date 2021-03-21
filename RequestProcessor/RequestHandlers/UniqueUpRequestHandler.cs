using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueUp;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Synonymize;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class UniqueUpRequestHandler : IRequestHandler {
        private Synonymizer _freeSynonymizer = new RussianSynonymizer();
        
        public override async Task<IActionResult> HandleRequest(string text) {
            UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(text);

            string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

            return new OkObjectResult(uniqueUpResponseJson);
        }
    }
}
