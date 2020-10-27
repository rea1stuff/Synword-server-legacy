using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    { "action", "GET_BALANCE" },
                    { "key", api.APIkey }
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
