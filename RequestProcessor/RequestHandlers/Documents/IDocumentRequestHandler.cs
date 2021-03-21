using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers.Documents {
    interface IDocumentRequestHandler {
        public Task<IActionResult> HandleRequest();
    }
}
