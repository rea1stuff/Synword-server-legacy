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
    public class UserApplicationDataHandler : IUserDataHandler<UserApplicationDataModel> {
        IDao<UserApplicationDataModel> _db;

        public UserApplicationDataHandler() {
            _db = new UserApplicationDataDao();
        }

        public void CreateUser(string uId) {
            string date = DateTime.Now.ToString();

            UserApplicationDataModel userData = new UserApplicationDataModel {
                uId = uId,
                isPremium = false,
                coins = UserLimits.Coins,
                uniqueCheckMaxSymbolLimit = UserLimits.UniqueCheckMaxSymbolLimit,
                uniqueUpMaxSymbolLimit = UserLimits.UniqueUpMaxSymbolLimit,
                documentUniqueCheckMaxSymbolLimit = UserLimits.DocumentUniqueCheckMaxSymbolLimit,
                documentUniqueUpMaxSymbolLimit = UserLimits.DocumentUniqueUpMaxSymbolLimit,
                lastVisitDate = date,
                creationDate = date
            };

            _db.Create(userData);
        }

        public UserApplicationDataModel GetUserData(string uId) {
            return _db.GetUserDataById(uId);
        }

        public void SetUserData(UserApplicationDataModel userData) {
            _db.SetUserData(userData);
        }

        public void DeleteUserData(string uId) {
            _db.DeleteUserData(uId);
        }

        public void SetPremium(string uId) {
            UserApplicationDataModel userData = _db.GetUserDataById(uId);

            userData.isPremium = true;
            userData.uniqueCheckMaxSymbolLimit = PremiumUserLimits.UniqueCheckMaxSymbolLimit;
            userData.uniqueUpMaxSymbolLimit = PremiumUserLimits.UniqueUpMaxSymbolLimit;
            userData.documentUniqueCheckMaxSymbolLimit = PremiumUserLimits.DocumentUniqueCheckMaxSymbolLimit;
            userData.documentUniqueUpMaxSymbolLimit = PremiumUserLimits.DocumentUniqueUpMaxSymbolLimit;

            if (userData.coins < PremiumUserLimits.Coins) {
                userData.coins = PremiumUserLimits.Coins;
            }
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
