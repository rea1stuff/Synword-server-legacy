using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UniqueCheck
{
    public class UniqueCheckFromContentWatchAPI
    {

        private static ContentWatchAPI_Model api = new ContentWatchAPI_Model();

        public async Task<string> postReqest(string text)
        {

            string responseString = "";

            HttpClient httpClient = new HttpClient();

            Dictionary<string, string> values = new Dictionary<string, string> {
                    { "key", api.APIkey },
                    { "text", text },
                    { "test", "1" }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("https://content-watch.ru/public/api/", content);

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }
    }
}
