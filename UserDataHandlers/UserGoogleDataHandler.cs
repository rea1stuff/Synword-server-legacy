using System;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.UserDataHandlers {
    public class UserGoogleDataHandler {
        IDao<UserGoogleDataModel> _db;

        public UserGoogleDataHandler() {
            _db = new UserGoogleDataDao();
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
