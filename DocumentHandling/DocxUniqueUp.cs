using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SynWord_Server_CSharp.Synonymize;

namespace SynWord_Server_CSharp.DocumentUniqueUp
{
    public class DocxUniqueUp
    {
        ISynonymizer _synonymizer = new FreeSynonymizer();

        public void UniqueUp(string documentPath)
        {
            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>())
                {
                    text.Text = _synonymizer.Synonymize(text.Text);
                }
            }
        }
    }
}
