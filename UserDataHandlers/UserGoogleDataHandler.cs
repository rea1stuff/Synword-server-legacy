using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.UserDataHandlers {
    public class UserGoogleDataHandler : IUserDataHandler<UserGoogleDataModel> {
        IDao<UserGoogleDataModel> _db;

        public UserGoogleDataHandler() {
            _db = new UserGoogleDataDao();
        }

        public void CreateUser(string accessToken) {
            UserGoogleDataModel userData = GoogleAuthApi.GetGoogleUserModel(accessToken);
            _db.Create(userData);
        }

        public UserGoogleDataModel GetUserData(string uId) {
            return _db.GetUserDataById(uId);
        }

        public void SetUserData(UserGoogleDataModel userData) {
            _db.SetUserData(userData);
        }

        public void DeleteUserData(string uId) {
            _db.DeleteUserData(uId);
        }
        public bool IsUserExist(string uId) {
            try {
                _db.GetUserDataById(uId);
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}
