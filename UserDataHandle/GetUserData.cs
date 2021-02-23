using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.UserData {
    public class GetUserData {
        readonly private IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        private string uId;

        public GetUserData(string uId) {
            this.uId = uId;
        }

        public string GetAllUserData() {
            List<BsonDocument> userData = GetData();

            if (userData.Count == 0) {
                throw new Exception("User data does not exist");
            }

            UserDataModel userDataModel = new UserDataModel();

            userDataModel.uId = userData[0]["uId"].ToString();
            userDataModel.isPremium = userData[0]["isPremium"].ToBoolean();
            userDataModel.uniqueCheckRequests = userData[0]["uniqueCheckRequests"].ToInt32();
            userDataModel.uniqueUpRequests = userData[0]["uniqueUpRequests"].ToInt32();
            userDataModel.documentUniqueUpRequests = userData[0]["documentUniqueUpRequests"].ToInt32();
            userDataModel.documentMaxSymbolLimit = userData[0]["documentMaxSymbolLimit"].ToInt32();
            userDataModel.uniqueCheckMaxSymbolLimit = userData[0]["uniqueCheckMaxSymbolLimit"].ToInt32();
            userDataModel.uniqueUpMaxSymbolLimit = userData[0]["uniqueUpMaxSymbolLimit"].ToInt32();
            userDataModel.creationDate = userData[0]["creationDate"].ToString();

            return userDataModel.ToJson();
        }

        public int GetUniqueCheckRequests() {
            return Get("uniqueCheckRequests");
        }

        public int GetUniqueUpRequests() {
            return Get("uniqueUpRequests");
        }

        public int GetDocumentUniqueUpRequests() {
            return Get("documentUniqueUpRequests");
        }
        public int GetDocumentMaxSymbolLimit() {
            return Get("documentMaxSymbolLimit");
        }
        public int GetUniqueCheckMaxSymbolLimit() {
            return Get("uniqueCheckMaxSymbolLimit");
        }

        public int GetUniqueUpMaxSymbolLimit() {
            return Get("uniqueUpMaxSymbolLimit");
        }

        public string GetCreationDate() {
            List<BsonDocument> userData = GetData();

            return userData[0]["creationDate"].ToString();
        }

        public bool isPremium() {
            List<BsonDocument> userData = GetData();

            if (userData[0]["isPremium"].AsBoolean) {
                return true;
            } else {
                return false;
            }
        }

        private int Get(string valueName) {
            List<BsonDocument> userData = GetData();

            if (userData.Count == 0) {
                throw new Exception("User data does not exist");
            }

            return userData[0][valueName].ToInt32();
        }

        private List<BsonDocument> GetData() {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uId", uId);
            List<BsonDocument> userData = collection.Find(filter).ToList();
            return userData;
        }
    }
}
