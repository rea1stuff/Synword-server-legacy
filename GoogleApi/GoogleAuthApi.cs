using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.GoogleApi {
    public class GoogleAuthApi {
        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}";

        public static string GetUserId(string accessToken) {
            var googleUserModel = GetGoogleUserModel(accessToken);
            return googleUserModel.uId;
        }

        public static UserGoogleDataModel GetGoogleUserModel(string accessToken) {
            HttpClient httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, accessToken));

            HttpResponseMessage httpResponseMessage;
            try {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK) {
                throw new InvalidAccessTokenException();
            }
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;

            var googleUserModel = JsonConvert.DeserializeObject<UserGoogleDataModel>(response);

            return googleUserModel;
        }
    }
}
