using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SynWord_Server_CSharp.UserData
{
    public class GetUserData
    {
        readonly private IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        private string uId;

        public GetUserData(string uId) 
        {
            this.uId = uId;
        }

        public string GetAllUserData()
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> userData = collection.Find(filter).ToList();
            if (userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }
            return userData.ToJson();
        }

        public int GetUniqueCheckRequests()
        {
            return Get("uniqueCheckRequests");
        }

        public int GetUniqueUpRequests()
        {
            return Get("uniqueUpRequests");
        }

        public int GetDocumentUniqueUpRequests()
        {
            return Get("documentUniqueUpRequests");
        }
        public int GetUniqueCheckMaxSymbolLimit()
        {
            return Get("uniqueCheckMaxSymbolLimit");
        }

        public int GetUniqueUpMaxSymbolLimit()
        {
            return Get("uniqueUpMaxSymbolLimit");
        }

        public string GetCreationDate()
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> user = collection.Find(filter).ToList();

            return user[0]["creationDate"].ToString();
        }

        public bool is24HoursPassed()
        {
            DateTime inputDate = DateTime.Parse(GetCreationDate());
            if ((DateTime.Now - inputDate) > new TimeSpan(24, 0, 0))
            {
                return true;
            }
            return false;
        }

        private int Get(String valueName)
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> user = collection.Find(filter).ToList();
            if (user.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            return user[0][valueName].ToInt32();
        }
    }
}
