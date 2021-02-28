using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model.Purchase {
    public class PurchaseModel {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("purchaseToken")]
        public string PurchaseToken { get; set; }
    }
}
