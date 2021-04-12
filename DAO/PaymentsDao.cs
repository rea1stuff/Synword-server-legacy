using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Model.UserPayment;
using System.Collections.Generic;
using System.Linq;

namespace SynWord_Server_CSharp.DAO {
    public class PaymentsDao : IDao<PaymentModel> {
        private IMongoDatabase _db;
        private IMongoCollection<BsonDocument> _collection;

        public PaymentsDao() {
            _db = DB.client.GetDatabase("synword");
            _collection = _db.GetCollection<BsonDocument>("paymentData");
        }
        public void Create(PaymentModel userData) {
            _collection.InsertOne(userData.ToBsonDocument());
        }

        public void DeleteUserData(string uId) {
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("uId", uId);

            _collection.DeleteOne(deleteFilter);
        }

        public List<PaymentModel> GetAllUsersData() {
            List<BsonDocument> userData = _collection.Find(new BsonDocument()).ToList(); ;

            List<PaymentModel> userDataModel = new List<PaymentModel>();

            foreach (BsonDocument user in userData) {
                var userDeserialized = BsonSerializer.Deserialize<PaymentModel>(user);
                userDataModel.Add(userDeserialized);
            }

            return userDataModel;
        }

        public PaymentModel GetUserDataById(string uId) {
            var filter = new BsonDocument("uId", uId);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                throw new UserDoesNotExistException();
            }

            PaymentModel userDataModel = BsonSerializer.Deserialize<PaymentModel>(userData);

            return userDataModel;
        }

        public void SetUserData(PaymentModel userData) {
            var filter = new BsonDocument("uId", userData.uId);
            _collection.UpdateOne(
                filter,
                Builders<BsonDocument>.Update.Push("orders", userData.orders.First()));
        }

        public bool IsUserExist(string uId) {
            var filter = new BsonDocument("uId", uId);
            BsonDocument userData = _collection.Find(filter).FirstOrDefault();

            if (userData == null) {
                return false;
            }
            return true;
        }
    }
}
