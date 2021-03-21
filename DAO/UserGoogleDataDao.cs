using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Model.UserData;
using System.Collections.Generic;
using System.Linq;

namespace SynWord_Server_CSharp.DAO {
    public class UserGoogleDataDao : IDao<UserGoogleDataModel> {
        private IMongoDatabase _db;
        private IMongoCollection<BsonDocument> _collection;

        public UserGoogleDataDao() {
            _db = DB.client.GetDatabase("synword");
            _collection = _db.GetCollection<BsonDocument>("userGoogleData");
        }

        public void Create(UserGoogleDataModel userData) {
            BsonDocument userDataDefaults = new BsonDocument{
                { "uId",  userData.uId},
                { "email", userData.email },
                { "verified_email", userData.verified_email },
                { "name", userData.name },
                { "given_name", userData.given_name },
                { "family_name", userData.family_name },
                { "picture", userData.picture },
                { "locale", userData.locale }
            };
            _collection.InsertOne(userDataDefaults);
        }

        public List<UserGoogleDataModel> GetAllUsersData() {
            List<BsonDocument> userData = _collection.Find(new BsonDocument()).ToList(); ;

            List<UserGoogleDataModel> userDataModel = new List<UserGoogleDataModel>();

            foreach (BsonDocument user in userData) {
                var userDeserialized = BsonSerializer.Deserialize<UserGoogleDataModel>(user);
                userDataModel.Add(userDeserialized);
            }

            return userDataModel;
        }

        public UserGoogleDataModel GetUserDataById(string uId) {
            var filter = new BsonDocument("uId", uId);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            UserGoogleDataModel userDataModel = BsonSerializer.Deserialize<UserGoogleDataModel>(userData);

            return userDataModel;
        }

        public void SetUserData(UserGoogleDataModel userData) {
            var filter = new BsonDocument("uId", userData.uId);
            _collection.UpdateOne(
                filter,
                Builders<BsonDocument>.Update.Set("uId", userData.uId)
                                        .Set("email", userData.email)
                                        .Set("verified_email", userData.verified_email)
                                        .Set("name", userData.name)
                                        .Set("given_name", userData.given_name)
                                        .Set("family_name", userData.family_name)
                                        .Set("picture", userData.picture)
                                        .Set("locale", userData.locale)
                                        );
        }

        public void DeleteUserData(string uId) {
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("uId", uId);

            _collection.DeleteOne(deleteFilter);
        }
    }
}
