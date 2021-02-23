using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UserPayment {
    public class UserPaymentModel {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("inAppItemId")]
        public string InAppItemId { get; set; }
        [JsonProperty("purchaseToken")]
        public string PurchaseToken { get; set; }
    }
}
