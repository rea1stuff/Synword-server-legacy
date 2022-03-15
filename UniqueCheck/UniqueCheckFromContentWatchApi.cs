using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Model.UniqueCheck;

namespace SynWord_Server_CSharp.UniqueCheck {
    public class UniqueCheckFromContentWatchApi {
        private readonly string _apiKey = ConfigurationManager.AppSettings["contentWatchApiKey"];

        public async Task<UniqueCheckResponseModel> PostReqest(string text) {
            HttpClient httpClient = new HttpClient();

            Dictionary<string, string> values = new Dictionary<string, string> {
                    { "key", _apiKey },
                    { "text", text },
                    { "test", "1" }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync(ContentWatch.ApiUrl, content);

            if (!response.IsSuccessStatusCode) {
                throw new Exception("Unique check error");
            }

            string responseString = await response.Content.ReadAsStringAsync();
            ContentWatchApiModel contentWatchModel = JsonConvert
                .DeserializeObject<ContentWatchApiModel>(responseString);

            if (contentWatchModel.Error != string.Empty) {
                throw new Exception(contentWatchModel.Error);
            }

            UniqueCheckResponseModel uniqueCheckResponse = new UniqueCheckResponseModel()
            {
                Percent = contentWatchModel.Percent,
                Matches = contentWatchModel.Matches
            };

            return uniqueCheckResponse;
        }
    }
}
