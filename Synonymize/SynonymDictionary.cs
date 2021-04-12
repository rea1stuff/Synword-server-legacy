using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.Synonymize {
    static class SynonymDictionary {
        static public List<RussianSynonym> russianDictionary;
        static public List<EnglishSynonym> englishDictionary;

        static SynonymDictionary() {
            russianDictionary = new List<RussianSynonym>();
            englishDictionary = new List<EnglishSynonym>();
        }

        static private void InitializeRussianDictionary() {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("russianSynonymsDictionary");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> synonyms = collection.Find(filter).Sort("{ id : 1 }").ToList();

            foreach (BsonDocument synonym in synonyms) {
                russianDictionary.Add(new RussianSynonym(synonym["id"].ToInt32(), synonym["word"].ToString(), synonym["synonymId"].ToInt32()));
            }
        }

        static private void InitializeEnglishDictionary() {
            MongoClient client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            IMongoDatabase database = client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("englishSynonymsDictionary");
            BsonDocument filter = new BsonDocument();

            List<BsonDocument> synonyms = collection.Find(filter).Sort("{ id : 1 }").ToList();

            foreach (BsonDocument synonym in synonyms) {
                List<string> synonymsList = new List<string>();

                foreach (string synonymString in synonym["synonyms"].AsBsonArray) {
                    synonymsList.Add(synonymString);
                }

                englishDictionary.Add(new EnglishSynonym(synonym["word"].ToString(), synonymsList));
            }
        }

        static public void InitializeDictionary() {
            InitializeRussianDictionary();
            InitializeEnglishDictionary();
        }
    }
}
