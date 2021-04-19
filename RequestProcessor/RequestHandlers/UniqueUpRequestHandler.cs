using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueUp;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Synonymize;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class UniqueUpRequestHandler : IRequestHandler {        
        public override async Task<IActionResult> HandleRequest(string text, Dictionary<string, dynamic> arguments = null) {
            Synonymizer synonymizer = new RussianSynonymizer();

            if (arguments != null) {
                if (arguments.ContainsKey("language")) {
                    string language = System.Convert.ToString(arguments["language"]);

                    if (language == "English") {
                        synonymizer = new EnglishSynonymizer();
                    }
                }
            }

            UniqueUpResponseModel uniqueUpResponse = synonymizer.Synonymize(text);

            string uniqueUpResponseJson = JsonConvert.SerializeObject(uniqueUpResponse);

            return new OkObjectResult(uniqueUpResponseJson);
        }
    }
}
