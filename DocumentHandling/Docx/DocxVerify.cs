using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SynWord_Server_CSharp.UserData;

namespace SynWord_Server_CSharp.DocumentHandling.Docx
{
    public class DocxLimitsCheck
    {
        private int _symbolCount;
        public int GetDocSymbolCount(string documentPath)
        {
            _symbolCount = 0;
            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true))
            {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>())
                {
                    _symbolCount += text.Text.Length;
                }
            }
            return _symbolCount;
        }
    }
}
