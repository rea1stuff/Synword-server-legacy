using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SynWord_Server_CSharp.Model.UserData;
using System.Collections.Generic;
using System.Linq;
using SynWord_Server_CSharp.Exceptions;

namespace SynWord_Server_CSharp.DAO {
    public class UserApplicationDataDao : IDao<UserApplicationDataModel> {
        private IMongoDatabase _db;
        private IMongoCollection<BsonDocument> _collection;

        public UserApplicationDataDao() {
            _db = DB.client.GetDatabase("synword");
            _collection = _db.GetCollection<BsonDocument>("userApplicationData");
        }

        public void Create(UserApplicationDataModel userData) {
            BsonDocument userDataDefaults = new BsonDocument{
                { "uId",  userData.uId},
                { "isPremium", userData.isPremium },
                { "coins", userData.coins },
                { "uniqueCheckMaxSymbolLimit", userData.uniqueCheckMaxSymbolLimit },
                { "uniqueUpMaxSymbolLimit", userData.uniqueUpMaxSymbolLimit },
                { "documentUniqueCheckMaxSymbolLimit", userData.documentUniqueCheckMaxSymbolLimit },
                { "documentUniqueUpMaxSymbolLimit", userData.documentUniqueUpMaxSymbolLimit },
                { "creationDate", userData.creationDate }
            };
            _collection.InsertOne(userDataDefaults);
        }

        public List<UserApplicationDataModel> GetAllUsersData() {
            List<BsonDocument> userData = _collection.Find(new BsonDocument()).ToList(); ;

            List<UserApplicationDataModel> userDataModel = new List<UserApplicationDataModel>();

            foreach (BsonDocument user in userData) {
                var userDeserialized = BsonSerializer.Deserialize<UserApplicationDataModel>(user);
                userDataModel.Add(userDeserialized);
            }

            return userDataModel;
        }

        public UserApplicationDataModel GetUserDataById(string uId) {
            var filter = new BsonDocument("uId", uId);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            UserApplicationDataModel userDataModel = BsonSerializer.Deserialize<UserApplicationDataModel>(userData);

            return userDataModel;
        }

        public UserApplicationDataModel GetUserDataByIp(string ip) {
            var filter = new BsonDocument("ip", ip);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            UserApplicationDataModel userDataModel = BsonSerializer.Deserialize<UserApplicationDataModel>(userData);

            return userDataModel;
        }

        public void SetUserData(UserApplicationDataModel userData) {
            var filter = new BsonDocument("uId", userData.uId);
            _collection.UpdateOne(
                filter, 
                Builders<BsonDocument>.Update.Set("uId", userData.uId)
                                        .Set("isPremium", userData.isPremium)
                                        .Set("coins", userData.coins)
                                        .Set("uniqueCheckMaxSymbolLimit", userData.uniqueCheckMaxSymbolLimit)
                                        .Set("uniqueUpMaxSymbolLimit", userData.uniqueUpMaxSymbolLimit)
                                        .Set("documentUniqueCheckMaxSymbolLimit", userData.documentUniqueCheckMaxSymbolLimit)
                                        .Set("documentUniqueUpMaxSymbolLimit", userData.documentUniqueUpMaxSymbolLimit)
                                        .Set("lastVisitDate", userData.lastVisitDate)
                                        .Set("creationDate", userData.creationDate)
                                        );
        }

        public void DeleteUserData(string uId) {
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("uId", uId);

            _collection.DeleteOne(deleteFilter);
        }
    }
}
