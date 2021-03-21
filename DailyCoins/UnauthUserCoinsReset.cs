using System.Collections.Generic;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.DailyCoins {
    public class UnauthUserCoinsReset {
        private IDao<UnauthUserApplicationDataModel> _unauthDao = new UnauthUserApplicationDataDao();

        public void ResetCoinsCount() {
            Reset();
        }

        public void Reset() {
            List<UnauthUserApplicationDataModel> allUsersData = _unauthDao.GetAllUsersData();
            List<UnauthUserApplicationDataModel> modifiedUsersData = new List<UnauthUserApplicationDataModel>();

            foreach(UnauthUserApplicationDataModel userData in allUsersData) {
                if (userData.coins < UserLimits.Coins) {
                    userData.coins = UserLimits.Coins;
                    modifiedUsersData.Add(userData);
                }
            }

            foreach(UnauthUserApplicationDataModel userData in modifiedUsersData) {
                _unauthDao.SetUserData(userData);
            }
        }
    }
}
