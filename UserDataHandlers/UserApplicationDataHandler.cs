using System;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.UserDataHandlers {
    public class UserApplicationDataHandler {
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
            _db.SetUserData(userData);
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
