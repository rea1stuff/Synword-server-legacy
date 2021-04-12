using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UserPayment {
    public class RequestPaymentModel {
        [JsonProperty("uId")]
        public string Uid { get; set; }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("purchaseToken")]
        public string PurchaseToken { get; set; }
    }
}
