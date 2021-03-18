using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueUp;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Synonymize;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class UniqueUpRequestHandler : IRequestHandler {
        ISynonymizer _freeSynonymizer = new FreeSynonymizer();
        public override async Task<IActionResult> HandleRequest(string text) {
            UniqueUpResponseModel uniqueUpResponse = _freeSynonymizer.Synonymize(text);

            string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

            return new OkObjectResult(uniqueUpResponseJson);
        }
    }
}
