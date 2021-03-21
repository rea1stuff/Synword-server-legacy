using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators {
    public class AuthValidationControl : IValidationControl {
        private UserApplicationDataModel _userData;
        private IUserDataHandler<UserApplicationDataModel> _userDataHandler;

        public AuthValidationControl(string uId) {
            _userDataHandler = new UserApplicationDataHandler();
            _userData = _userDataHandler.GetUserData(uId);
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
            _userDataHandler.SetUserData(_userData);
        }
    }
}
