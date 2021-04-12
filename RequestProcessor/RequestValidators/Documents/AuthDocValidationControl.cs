using SynWord_Server_CSharp.Model.UserData;
using Microsoft.AspNetCore.Http;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents {
    public class AuthDocValidationControl : IDocumentValidationControl {
        private UserApplicationDataModel _userData;
        private IUserApplicationDataDao _userDao;

        public AuthDocValidationControl(string uId, IFormFile file) : base(file) {
            _userDao = new UserApplicationDataDao();
            _userData = _userDao.GetUserDataById(uId);
        }

        protected override bool IsPremium() {
            return _userData.isPremium;
        }

        protected override int GetCoins() {
            return _userData.coins;
        }

        protected override int GetUniqueCheckMaxSymbolLimit() {
            return _userData.documentUniqueCheckMaxSymbolLimit;
        }

        protected override int GetUniqueUpMaxSymbolLimit() {
            return _userData.documentUniqueUpMaxSymbolLimit;
        }

        public override void SpendCoins(int price) {
            _userData.coins -= price;
            _userDao.SetUserData(_userData);
        }
    }
}
