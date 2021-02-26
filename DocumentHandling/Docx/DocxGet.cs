using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxGet {
        private int _symbolCount;

        public int GetDocSymbolCount(string documentPath) {
            _symbolCount = 0;

            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>()) {
                    _symbolCount += text.Text.Length;
                }
            }

            return _symbolCount;
        }
        public string GetDocText(string documentPath) {
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
