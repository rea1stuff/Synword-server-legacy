using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.GoogleApi;

namespace SynWord_Server_CSharp.UserData {
    public class UserDataHandle {
        readonly IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();
        string uId;

        public UserDataHandle(string uId) {
            this.uId = uId;
        }

        public bool IsUserExist() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uId", uId);
            List<BsonDocument> userData = collection.Find(filter).ToList();

            if (userData.Count == 0) {
                return false;
            }

            return true;
        }
        public void CreateUser() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");

            string date = DateTime.Now.ToString();

            BsonDocument userDataDefaults = new BsonDocument{
                { "uId",  uId},
                { "isPremium", false },
                { "uniqueCheckRequests", UserLimits.UniqueCheckRequests },
                { "uniqueUpRequests", UserLimits.UniqueUpRequests },
                { "documentMaxSymbolLimit", UserLimits.DocumentMaxSymbolLimit },
                { "uniqueCheckMaxSymbolLimit", UserLimits.UniqueCheckMaxSymbolLimit },
                { "uniqueUpMaxSymbolLimit", UserLimits.UniqueUpMaxSymbolLimit },
                { "creationDate", date }
            };
            collection.InsertOne(userDataDefaults);
        }
    }
}
