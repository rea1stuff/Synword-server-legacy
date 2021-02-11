using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.UserData
{
    public class GetUserData
    {
        readonly private IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        private string uId;
        private List<BsonDocument> _userData;

        public GetUserData(string uId) 
        {
            this.uId = uId;

            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            _userData = collection.Find(filter).ToList();
        }

        public string GetAllUserData()
        {
            if (_userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            UserDataModel userData = new UserDataModel();

            userData.uid = _userData[0]["uid"].ToString();
            userData.isPremium = _userData[0]["isPremium"].ToBoolean();
            userData.uniqueCheckRequests = _userData[0]["uniqueCheckRequests"].ToInt32();
            userData.uniqueUpRequests = _userData[0]["uniqueUpRequests"].ToInt32();
            userData.documentUniqueUpRequests = _userData[0]["documentUniqueUpRequests"].ToInt32();
            userData.documentMaxSymbolLimit = _userData[0]["documentMaxSymbolLimit"].ToInt32();
            userData.uniqueCheckMaxSymbolLimit = _userData[0]["uniqueCheckMaxSymbolLimit"].ToInt32();
            userData.uniqueUpMaxSymbolLimit = _userData[0]["uniqueUpMaxSymbolLimit"].ToInt32();
            userData.creationDate = _userData[0]["creationDate"].ToString();

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
        public int GetDocumentMaxSymbolLimit()
        {
            return Get("documentMaxSymbolLimit");
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
            return _userData[0]["creationDate"].ToString();
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

        public bool isPremium()
        {
            if (_userData[0]["isPremium"].AsBoolean)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int Get(String valueName)
        {
            if (_userData.Count == 0)
            {
                throw new Exception("User data does not exist");
            }

            return _userData[0][valueName].ToInt32();
        }
    }
}
