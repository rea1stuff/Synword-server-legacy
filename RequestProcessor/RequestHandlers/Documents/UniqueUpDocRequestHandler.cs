using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.UniqueCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SynWord_Server_CSharp.Model.UniqueUp;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.DocumentHandling.Docx;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers.Documents {
    public class UniqueUpDocRequestHandler : IDocumentRequestHandler {
        string _filePath;
        public UniqueUpDocRequestHandler(string filePath) {
            _filePath = filePath;
        }
        public async Task<IActionResult> HandleRequest() {
            DocxUniqueUp.UniqueUp(_filePath);

            string mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            FileStream stream = File.OpenRead(_filePath);

            return new FileStreamResult(stream, mimeType) {
                FileDownloadName = "Synword_UniqueUp"
            };
        }
    }
}
