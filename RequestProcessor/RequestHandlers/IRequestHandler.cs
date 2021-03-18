using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public abstract class IRequestHandler {
        public abstract Task<IActionResult> HandleRequest(string text);
    }
}
