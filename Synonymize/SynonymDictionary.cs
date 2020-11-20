using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.Synonymize {
    static class SynonymDictionary {
        static public List<Synonym> dictionary;

        static SynonymDictionary() {
            dictionary = new List<Synonym>();
        }

        static public void InitializeDictionary() {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("synonyms");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> synonyms = collection.Find(filter).Sort("{ id : 1 }").ToList();
            
            foreach (BsonDocument synonym in synonyms) {
                dictionary.Add(new Synonym(synonym["id"].ToInt32(), synonym["word"].ToString(), synonym["synonymId"].ToInt32()));
            }
        }
    }
}
