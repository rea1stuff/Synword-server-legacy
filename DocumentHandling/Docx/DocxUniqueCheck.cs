using System.Threading.Tasks;
using SynWord_Server_CSharp.UniqueCheck;
using SynWord_Server_CSharp.Model.UniqueCheck;

namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxUniqueCheck {
        public static async Task<UniqueCheckResponseModel> UniqueCheck(string filePath) {

            UniqueCheckApi uniqueCheck = new UniqueCheckApi();

            string docxText = DocxGet.GetText(filePath);

            UniqueCheckResponseModel response = await uniqueCheck.UniqueCheck(docxText);

            return response;
        }
    }
}
