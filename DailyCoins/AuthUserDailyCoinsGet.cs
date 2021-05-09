using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;
using System;

namespace SynWord_Server_CSharp.DailyCoins {
    public class AuthUserDailyCoinsGet : IDailyCoinsGet {
        private const int _defaultCoinsPerDay = 20;
        private const int _premiumCoinsPerDay = 60;
        IDao<UserApplicationDataModel> _db = new UserApplicationDataDao();
        UserApplicationDataModel _userData = new UserApplicationDataModel();
        string _uId;

        public AuthUserDailyCoinsGet(string uId) {
            _uId = uId;
            _userData = _db.GetUserDataById(_uId);
        }

        public bool Is24HoursPassed() {
            DateTime inputDate = DateTime.Parse(_userData.lastVisitDate);
            if ((DateTime.Now - inputDate) > new TimeSpan(24, 0, 0)) {
                return true;
            }
            return false;
        }

        public void GiveCoinsToUser() {
            if (_userData.isPremium) {
                _userData.coins += _premiumCoinsPerDay;
            } else {
                _userData.coins += _defaultCoinsPerDay;
            }
            _db.SetUserData(_userData);
        }
    }
}
