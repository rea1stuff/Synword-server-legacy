using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxGet {
        public static int GetSymbolCount(string documentPath) {
            int symbolCount = 0;

            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>()) {
                    symbolCount += text.Text.Length;
                }
            }

            return symbolCount;
        }

        public static string GetText(string documentPath) {
            string docText = "";

            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>()) {
                    docText += text.Text;
                }
            }

            return docText;
        }
    }
}
