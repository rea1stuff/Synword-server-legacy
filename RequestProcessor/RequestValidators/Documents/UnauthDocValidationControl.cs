using SynWord_Server_CSharp.Model.UserData;
using System;
using Microsoft.AspNetCore.Http;
using SynWord_Server_CSharp.DAO;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents {
    public class UnauthDocValidationControl : IDocumentValidationControl {
        private UnauthUserApplicationDataModel _userData;
        private IUnauthUserDao _userDao;

        public UnauthDocValidationControl(string ip, IFormFile file) : base(file) {
            _userDao = new UnauthUserApplicationDataDao();
            _userData = _userDao.GetUserDataById(ip);
        }

        protected override bool IsPremium() {
            return false;
        }

        protected override int GetCoins() {
            return _userData.coins;
        }

        protected override int GetUniqueCheckMaxSymbolLimit() {
            throw new NotImplementedException();
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
