using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UniqueCheck
{

    public class UniqueCheckFromContentWatchAPI
    {
        public string APIkey { get; set; } = "G50TjIzlIGXFzBa";

        private static ContentWatchAPI_Model contentWatchModel = new ContentWatchAPI_Model();

        public async Task<ActionResult> postReqest(string text)
        {

            string responseString = "";

            HttpClient httpClient = new HttpClient();

            Dictionary<string, string> values = new Dictionary<string, string> {
                    { "key", APIkey },
                    { "text", text },
                    { "test", "1" }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await httpClient.PostAsync("https://content-watch.ru/public/api/", content);

            if (!response.IsSuccessStatusCode)
            {
                return new BadRequestObjectResult("Server error");
            }

            responseString = await response.Content.ReadAsStringAsync();
            ContentWatchAPI_Model deserializedObj = JsonConvert.DeserializeObject<ContentWatchAPI_Model>(responseString);

            if (deserializedObj.error != String.Empty)
            {
                return new BadRequestObjectResult(deserializedObj.error);
            }

            var serializedObj = JsonConvert.SerializeObject(deserializedObj);

            return new OkObjectResult(serializedObj);
        }
    }
}
