using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Model.UniqueCheck;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SynWord_Server_CSharp.DocumentHandling.Docx
{
    public class DocxUniqueCheck
    {
        private UniqueCheckFromContentWatchApi _uniqueCheck = new UniqueCheckFromContentWatchApi();
        private string _docPath;
        public DocxUniqueCheck(string docPath)
        {
            this._docPath = docPath;
        }
        public async Task<UniqueCheckResponseModel> UniqueCheck()
        {
            DocxGet docxGet = new DocxGet();
            UniqueCheckApi uniqueCheck = new UniqueCheckApi();

            string docxText = docxGet.GetDocText(_docPath);
            
            UniqueCheckResponseModel response = await uniqueCheck.UniqueCheck(docxText);

            return response;
        }
    }
}
