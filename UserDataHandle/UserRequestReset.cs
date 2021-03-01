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
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> documentList = collection.Find(filter).ToList();

            foreach (BsonDocument document in documentList) {
                if (Convert.ToInt32(document["uniqueCheckRequests"]) < UserLimits.UniqueCheckRequests) {
                    filter = new BsonDocument("_id", new ObjectId(Convert.ToString(document["_id"])));
                    BsonDocument update = new BsonDocument("$set", new BsonDocument { { "uniqueCheckRequests", UserLimits.UniqueCheckRequests } });
                    collection.UpdateOne(filter, update);
                }
            }
        }

        public void ResetUpRequest() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> documentList = collection.Find(filter).ToList();

            foreach (BsonDocument document in documentList) {
                if (Convert.ToInt32(document["uniqueUpRequests"]) < UserLimits.UniqueUpRequests) {
                    filter = new BsonDocument("_id", new ObjectId(Convert.ToString(document["_id"])));
                    BsonDocument update = new BsonDocument("$set", new BsonDocument { { "uniqueUpRequests", UserLimits.UniqueUpRequests } });
                    collection.UpdateOne(filter, update);
                }
            }
        }

        public void ResetDocumentUniqueUpRequest() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> documentList = collection.Find(filter).ToList();

            foreach (BsonDocument document in documentList) {
                if (Convert.ToInt32(document["documentUniqueUpRequests"]) < UserLimits.DocumentUniqueUpRequests) {
                    filter = new BsonDocument("_id", new ObjectId(Convert.ToString(document["_id"])));
                    BsonDocument update = new BsonDocument("$set", new BsonDocument { { "documentUniqueUpRequests", UserLimits.DocumentUniqueUpRequests } });
                    collection.UpdateOne(filter, update);
                }
            }
        }
    }
}
