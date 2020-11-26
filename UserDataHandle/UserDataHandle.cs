using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SynWord_Server_CSharp.UserData
{
    public class UserDataHandle
    {
        readonly private IMongoClient _client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
        string uId;

        public UserDataHandle(string uId)
        {
            this.uId = uId;
        }

        public void ResetDefaults() 
        {
            GetUserData getUserData = new GetUserData(uId);
            SetUserData setUserData = new SetUserData(uId);

            if (getUserData.GetUniqueCheckRequests() < 10)
            {
                setUserData.SetUniqueCheckRequest(10);
            }

            if (getUserData.GetUniqueUpRequests() < 1000)
            {
                setUserData.SetUniqueUpRequest(1000);
            }

            if (getUserData.GetDocumentUniqueUpRequests() < 30)
            {
                setUserData.SetDocumentUniqueUpRequests(30);
            }

            setUserData.SetCreationDateForToday();
        }

        public void CheckUserIdExistIfNotCreate()
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");
            var filter = new BsonDocument("uid", uId);
            List<BsonDocument> userData = collection.Find(filter).ToList();

            if (userData.Count == 0)
            {
                UploadIdToDB();
            }
        }

        private void UploadIdToDB()
        {
            IMongoDatabase database = _client.GetDatabase("synword");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("userData");

            string date = DateTime.Now.ToString();

            BsonDocument userDataDefaults = new BsonDocument{
                { "uid",  uId},
                { "isPremium", false },
                { "uniqueCheckRequests", 10 },
                { "uniqueUpRequests", 1000 },
                { "documentUniqueUpRequests", 30 },
                { "uniqueCheckMaxSymbolLimit", 20000 },
                { "uniqueUpMaxSymbolLimit", 20000 },
                { "creationDate", date }
            };
            collection.InsertOne(userDataDefaults);
        }

    }
}
