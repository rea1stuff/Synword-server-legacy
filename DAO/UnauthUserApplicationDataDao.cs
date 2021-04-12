using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Model.UserData;
using System.Collections.Generic;
using System.Linq;

namespace SynWord_Server_CSharp.DAO {
    public class UnauthUserApplicationDataDao : IUnauthUserDao {
        private IMongoDatabase _db;
        private IMongoCollection<BsonDocument> _collection;

        public UnauthUserApplicationDataDao() {
            _db = DB.client.GetDatabase("synword");
            _collection = _db.GetCollection<BsonDocument>("unauthUserApplicationData");
        }

        public void Create(UnauthUserApplicationDataModel userData) {
            _collection.InsertOne(userData.ToBsonDocument());
        }

        public List<UnauthUserApplicationDataModel> GetAllUsersData() {
            List<BsonDocument> userData = _collection.Find(new BsonDocument()).ToList(); ;

            List<UnauthUserApplicationDataModel> userDataModel = new List<UnauthUserApplicationDataModel>();

            foreach (BsonDocument user in userData) {
                var userDeserialized = BsonSerializer.Deserialize<UnauthUserApplicationDataModel>(user);
                userDataModel.Add(userDeserialized);
            }

            return userDataModel;
        }

        public UnauthUserApplicationDataModel GetUserDataById(string uId) {
            var filter = new BsonDocument("uId", uId);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            UnauthUserApplicationDataModel userDataModel = BsonSerializer.Deserialize<UnauthUserApplicationDataModel>(userData);

            return userDataModel;
        }

        public UnauthUserApplicationDataModel GetUserDataByIp(string ip) {
            var filter = new BsonDocument("ip", ip);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            UnauthUserApplicationDataModel userDataModel = BsonSerializer.Deserialize<UnauthUserApplicationDataModel>(userData);

            return userDataModel;
        }

        public void SetUserData(UnauthUserApplicationDataModel userData) {
            var filter = new BsonDocument("uId", userData.uId);
            _collection.UpdateOne(
                filter,
                Builders<BsonDocument>.Update.Set("uId", userData.uId)
                                        .Set("ip", userData.ip)
                                        .Set("coins", userData.coins)
                                        .Set("uniqueCheckMaxSymbolLimit", userData.uniqueCheckMaxSymbolLimit)
                                        .Set("uniqueUpMaxSymbolLimit", userData.uniqueUpMaxSymbolLimit)
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
