using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using SynWord_Server_CSharp.Constants;

namespace SynWord_Server_CSharp.UserData {
    public class UserRequestReset {
        readonly private IMongoClient _client;

        public UserRequestReset() {
            _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        }

        public void ResetUniqueCheckRequest() {
            Reset("uniqueCheckRequests", UserLimits.UniqueCheckRequests, PremiumUserLimits.UniqueCheckRequests);
        }

        public void ResetUpRequest() {
            Reset("uniqueUpRequests", UserLimits.UniqueUpRequests, PremiumUserLimits.UniqueUpRequests);
        }

        public void Reset(string fieldName, int value, int premiumValue) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> documentList = collection.Find(filter).ToList();

            foreach (BsonDocument document in documentList) {
                if (Convert.ToBoolean(document["isPremium"]) == true) {
                    value = premiumValue;
                }
                if (Convert.ToInt32(document[fieldName]) < value) {
                    filter = new BsonDocument("_id", new ObjectId(Convert.ToString(document["_id"])));
                    BsonDocument update = new BsonDocument("$set", new BsonDocument { { fieldName, value } });
                    collection.UpdateOne(filter, update);
                }
            }
        }
    }
}
