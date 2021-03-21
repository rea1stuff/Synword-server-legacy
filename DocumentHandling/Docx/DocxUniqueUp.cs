using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SynWord_Server_CSharp.Synonymize;

namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxUniqueUp {
        public static void UniqueUp(string documentPath) {
            Synonymizer _synonymizer = new RussianSynonymizer();

            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>()) {
                    text.Text = _synonymizer.Synonymize(text.Text).Text;
                }
            }
        }
    }
}
