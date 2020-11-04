using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Logging {
    public class FreeSynonimizerUsageLog {
        readonly private IMongoClient client;

        public FreeSynonimizerUsageLog() {
            client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        }

        public void CheckIpExistsIfNotThenCreate(string ip) {
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingFreeSynonymizer");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            if (documents.Count == 0) {
                UploadIpToDataBase(ip);
            }
        }

        private void UploadIpToDataBase(string ip) {
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingFreeSynonymizer");

            BsonDocument document = new BsonDocument{
                { "ip", ip },
                { "usesForAllTime", 0 },
                { "usesForTheLastDay", 0 }
            };

            collection.InsertOne(document);
        }

        public int GetUsesIn24Hours(string ip) {
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingFreeSynonymizer");
            BsonDocument filter = new BsonDocument("ip", ip);
            List<BsonDocument> documents = collection.Find(filter).ToList();

            return documents.Count != 0 ? documents[0]["usesForTheLastDay"].ToInt32() : 0;
        }

        public void IncrementNumberOfUsesIn24Hours(string ip) {
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("usingFreeSynonymizer");
            BsonDocument filter = new BsonDocument("ip", ip);
            BsonDocument update = new BsonDocument("$inc", new BsonDocument { { "usesForAllTime", 1 }, { "usesForTheLastDay", 1 } });
            collection.UpdateOne(filter, update);
        }
    }
}
