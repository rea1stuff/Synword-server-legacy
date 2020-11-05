using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SynWord_Server_CSharp.Logging {
    public class VisitsLog {
        readonly private IMongoClient _client;

        public VisitsLog() {
            _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        }

        public void CheckIpExistsIfNotThenCreate(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("visits");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            if (documents.Count == 0) {
                UploadIpToDataBase(ip);
            }
        }

        private void UploadIpToDataBase(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("visits");

            BsonDocument document = new BsonDocument{
                { "ip", ip },
                { "visitsForAllTime", 0 },
                { "visitsForTheLastDay", 0 }
            };

            collection.InsertOne(document);
        }

        public int GetVisitsIn24Hours(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("visits");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            return documents.Count != 0 ? documents[0]["visitsForTheLastDay"].ToInt32() : 0;
        }

        public void IncrementNumberOfVisitsIn24Hours(string ip) {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("visits");
            BsonDocument filter = new BsonDocument("ip", ip);
            BsonDocument update = new BsonDocument("$inc", new BsonDocument { { "visitsAllTime", 1 }, { "visitsForTheLastDay", 1 } });
            collection.UpdateOne(filter, update);
        }
    }
}
