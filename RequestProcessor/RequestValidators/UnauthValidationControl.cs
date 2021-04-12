using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators {
    public class UnauthValidationControl : IValidationControl {
        private UnauthUserApplicationDataModel _userData;
        private IUnauthUserDao _userDao;
        
        public UnauthValidationControl(string uId) {
            _userDao = new UnauthUserApplicationDataDao();
            _userData = _userDao.GetUserDataById(uId);
        }

        protected override int GetCoins() {
            return _userData.coins;
        }

        protected override int GetUniqueCheckMaxSymbolLimit() {
            return _userData.uniqueCheckMaxSymbolLimit;
        }

        protected override int GetUniqueUpMaxSymbolLimit() {
            return _userData.uniqueUpMaxSymbolLimit;
        }

        public override void SpendCoins(int price) {
            _userData.coins -= price;
            _userDao.SetUserData(_userData);
        }
    }
}
