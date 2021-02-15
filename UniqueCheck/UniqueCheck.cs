using SynWord_Server_CSharp.Model.UniqueCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.UniqueCheck
{
    public class UniqueCheckApi
    {
        private UniqueCheckFromContentWatchApi _uniqueCheck = new UniqueCheckFromContentWatchApi();
        private int _contentWatchApiInputRestriction = 19999;

        public async Task<UniqueCheckResponseModel> UniqueCheck(string text)
        {
            //В каждом элементе текст до 20 тыс. символов (огрничение api)
            List<string> splitText = GetSplitText(text);

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
                ratioFromFirstPart.Add((splitText[i + 1].Length * 100.0) / splitText[0].Length);
            }

            //На основе данных ratioFromFirstPart корректируем процент
            List<double> correction = new List<double>();

            for (int i = 0; i < (splitUniqueCheckResponse.Count - 1); i++)
            {
                correction.Add((splitUniqueCheckResponse[i + 1].Percent / 100) * ratioFromFirstPart[i]);
            }

            double sum = splitUniqueCheckResponse[0].Percent;

            for (int i = 0; i < (splitUniqueCheckResponse.Count - 1); i++)
            {
                sum += correction[i];
            }

            //Среднее арифметическое
            double average = sum / splitUniqueCheckResponse.Count;

            UniqueCheckResponseModel response = new UniqueCheckResponseModel();
            response.Percent = (float)average;
            response.Matches = splitUniqueCheckResponse[0].Matches;

            return response;
        }
        private List<string> GetSplitText(string text)
        {
            List<string> splitText = new List<string>();
            int endIndex = _contentWatchApiInputRestriction;
            bool flag = true;
            while (flag)
            {
                if (text.Length < _contentWatchApiInputRestriction)
                {
                    endIndex = text.Length;
                    flag = false;
                }

                string temp = text.Substring(0, endIndex);
                splitText.Add(temp);

                text = text.Substring(endIndex);
            }

            return splitText;
        }
    }
}
