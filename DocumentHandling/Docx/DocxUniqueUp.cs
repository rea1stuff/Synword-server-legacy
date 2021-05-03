using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.Constants;

namespace SynWord_Server_CSharp.DocumentHandling.Docx {
    public class DocxUniqueUp {
        public static void UniqueUp(string documentPath, string lang) {
            Synonymizer _synonymizer = lang switch {
                Language.Russian => new RussianSynonymizer(),
                Language.English => new EnglishSynonymizer(),
                _ => throw new Exception("Invalid lang code"),
            };

            using (WordprocessingDocument document = WordprocessingDocument.Open(documentPath, true)) {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>()) {
                    text.Text = _synonymizer.Synonymize(text.Text).Text;
                }
            }
        }
    }
}
