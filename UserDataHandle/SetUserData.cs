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
        private IMongoCollection<BsonDocument> _collection;
        private List<BsonDocument> _userData;

        public SetUserData(string uId)
        {
            this.uId = uId;
            IMongoDatabase database = _client.GetDatabase("synword");
            _collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            _userData = _collection.Find(filter).ToList();
        }

        public void SetCreationDateForToday()
        {
            var filter = new BsonDocument("uid", uId);

            if (_userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            string date = DateTime.Now.ToString();

            var update = Builders<BsonDocument>.Update.Set("creationDate", date);

            _collection.UpdateOne(filter, update);
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
            var filter = new BsonDocument("uid", uId);

            if (_userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            var update = Builders<BsonDocument>.Update.Set(valueName, count);

            _collection.UpdateOne(filter, update);
        }

        private void SetRequests(String valueName, int count)
        {
            var filter = new BsonDocument("uid", uId);

            if (_userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            var update = Builders<BsonDocument>.Update.Set(valueName, count);

            _collection.UpdateOne(filter, update);
        }
    }
}
