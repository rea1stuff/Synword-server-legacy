using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SynWord_Server_CSharp.UserData
{
    public class SetUserData
    {
        readonly private IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        private string uId;

        public SetUserData(string uId)
        {
            this.uId = uId;
        }

        public void SetCreationDateForToday()
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> user = collection.Find(filter).ToList();

            if (user.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            string date = DateTime.Now.ToString();

            var update = Builders<BsonDocument>.Update.Set("creationDate", date);

            collection.UpdateOne(filter, update);
        }

        public void SetUniqueCheckRequest(int count)
        {
            SetRequests("uniqueCheckRequests", count);
        }

        public void SetUniqueUpRequest(int count)
        {
            SetRequests("uniqueUpRequests", count);
        }

        public void SetDocumentUniqueUpRequests(int count)
        {
            SetRequests("documentUniqueUpRequests", count);
        }
        public void SetUniqueCheckMaxSymbolLimit(int count)
        {
            SetMaxSymbolLimit("uniqueCheckMaxSymbolLimit", count);
        }

        public void SetUniqueUpMaxSymbolLimit(int count)
        {
            SetMaxSymbolLimit("uniqueUpMaxSymbolLimit", count);
        }

        private void SetMaxSymbolLimit(String valueName, int count)
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> user = collection.Find(filter).ToList();

            if (user.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            var update = Builders<BsonDocument>.Update.Set(valueName, count);

            collection.UpdateOne(filter, update);
        }

        private void SetRequests(String valueName, int count)
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> user = collection.Find(filter).ToList();

            if (user.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            int oldCount = user[0][valueName].ToInt32();

            int newCount = oldCount + count;

            var update = Builders<BsonDocument>.Update.Set(valueName, newCount);

            collection.UpdateOne(filter, update);
        }
    }
}
