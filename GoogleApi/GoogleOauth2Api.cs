using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.GoogleApi
{
    public class GoogleOauth2Api
    {
        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}";
        public bool IsTokenExist(string uId)
        {
            HttpClient httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, uId));

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        public GoogleUserModel GetUserDetails(string accessToken)
        {
            var googleUserModel = GetGoogleUserModel(accessToken);

            return googleUserModel;
        }

        public string GetUserId(string accessToken)
        {
            var googleUserModel = GetGoogleUserModel(accessToken);

            return googleUserModel.id;
        }

        private GoogleUserModel GetGoogleUserModel(string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, accessToken));

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidTokenException();
            }
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;

            var googleUserModel = JsonConvert.DeserializeObject<GoogleUserModel>(response);

            return googleUserModel;
        }
    }
}
