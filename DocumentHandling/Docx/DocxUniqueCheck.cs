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
        private List<int> _symbolCount = new List<int>();
        public DocxUniqueCheck(string docPath)
        {
            this._docPath = docPath;
        }
        public async Task<UniqueCheckResponseModel> UniqueCheck()
        {
            //В каждом элементе текст до 20 тыс. символов (огрничение api)
            List<string> splitText = GetSplitDocText(_docPath);

            //В каждом элементе храним процент уникальности элемента splitText
            List<UniqueCheckResponseModel> splitUniqueCheckResponse = new List<UniqueCheckResponseModel>();

            for (int i = 0; i < splitText.Count; i++)
            {
                UniqueCheckResponseModel uniqueCheckResponse = await _uniqueCheck.PostReqest(splitText[i]);
                splitUniqueCheckResponse.Add(uniqueCheckResponse);
            }

            //Какой процент от 20 тыс. занимают остальные части
            List<double> ratioFromFirstPart = new List<double>();

            for (int i = 0; i < (splitUniqueCheckResponse.Count - 1); i++)
            {
                ratioFromFirstPart.Add((_symbolCount[0] / _symbolCount[i + 1]) * 100.0);
            }

            //На основе данных ratioFromFirstPart корректируем процент
            List<double> correction = new List<double>();

            for (int i = 0; i < (splitUniqueCheckResponse.Count - 1); i++)
            {
                correction.Add((splitUniqueCheckResponse[i].Percent / 100) * ratioFromFirstPart[i]);
            }

            double sum = splitUniqueCheckResponse[0].Percent;

            for (int i = 0; i < (splitUniqueCheckResponse.Count - 1); i++)
            {
                sum += correction[i];
            }

            double average = sum / splitUniqueCheckResponse.Count;

            UniqueCheckResponseModel response = new UniqueCheckResponseModel();
            response.Percent = (float)average;
            response.Matches = splitUniqueCheckResponse[0].Matches;

            return response;
        }

        private List<string> GetSplitDocText(string docPath)
        {
            string temp = "";
            List<string> splitText = new List<string>();

            using (WordprocessingDocument document = WordprocessingDocument.Open(docPath, true))
            {
                Body body = document.MainDocumentPart.Document.Body;

                foreach (Text text in body.Descendants<Text>())
                {
                    temp += text.Text;
                    if (temp.Length > 19500)
                    {
                        _symbolCount.Add(temp.Length);
                        splitText.Add(temp);
                        temp = "";
                    }
                }
            }
            if (temp.Length > 5000)
            {
                _symbolCount.Add(temp.Length);
                splitText.Add(temp);
            }
            return splitText;
        }
    }
}
