using System.Threading.Tasks;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Model.UniqueCheck;
namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxUniqueCheck {
        private string _docPath;
        public DocxUniqueCheck(string docPath) {
            _docPath = docPath;
        }
        public async Task<UniqueCheckResponseModel> UniqueCheck() {
            DocxGet docxGet = new DocxGet();
            UniqueCheckApi uniqueCheck = new UniqueCheckApi();

            string docxText = docxGet.GetDocText(_docPath);
            
            UniqueCheckResponseModel response = await uniqueCheck.UniqueCheck(docxText);

            return response;
        }
    }
}
