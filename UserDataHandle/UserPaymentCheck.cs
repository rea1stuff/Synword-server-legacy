using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.UserData
{
    public class UserPaymentCheck
    {
        String serviceAccountEmail = "p-125-706@pc-api-6391117114651204772-21.iam.gserviceaccount.com";
        X509Certificate2 certificate = new X509Certificate2(@"fileName", "notasecret", X509KeyStorageFlags.Exportable);

        public bool PaymentCheck(String inAppItemId, String purchaseToken)
        {
            ServiceAccountCredential credential = new ServiceAccountCredential(
           new ServiceAccountCredential.Initializer(serviceAccountEmail)
           {
               Scopes = new[] { "https://www.googleapis.com/auth/androidpublisher" }
           }.FromCertificate(certificate));


            var service = new AndroidPublisherService(
           new BaseClientService.Initializer()
           {
               HttpClientInitializer = credential,
               ApplicationName = "GooglePlay API Sample",
           });
            // try catch this function because if you input wrong params ( wrong token) google will return error.
            var request = service.Purchases.Products.Get("synword", inAppItemId, purchaseToken);
            var purchaseState = request.Execute();

            // var request = service.Purchases.Products.Get(
            //"your-package-name", "your-inapp-item-id", "purchase-token"); get purchase'status

            Console.WriteLine(JsonConvert.SerializeObject(purchaseState));

            return true;
        }
    }
}
