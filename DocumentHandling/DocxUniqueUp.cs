using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Synonymize;

namespace DocumentUniqueUp
{
    public class DocxUniqueUp
    {
        ISynonymizer synonymizer = new FreeSynonymizer();
        public void UniqueUp(string documentPath)
        {
            using (WordprocessingDocument doc =
                  WordprocessingDocument.Open(documentPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;

                foreach (var text in body.Descendants<Text>())
                {
                    text.Text = synonymizer.Synonymize(text.Text);
                }
            }
        }
    }
}
