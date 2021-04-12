using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.UserPayment {
    [BsonIgnoreExtraElements]
    public class PaymentModel {
        public string uId { get; set; }
        public List<Order> orders { get; set; } = new List<Order>();
    }
}
