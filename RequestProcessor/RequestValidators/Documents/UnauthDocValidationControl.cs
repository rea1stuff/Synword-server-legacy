using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.UserDataHandlers;
using System;
using Microsoft.AspNetCore.Http;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents {
    public class UnauthDocValidationControl : IDocumentValidationControl {
        private UnauthUserApplicationDataModel _userData;
        private IUserDataHandler<UnauthUserApplicationDataModel> _userDataHandler;

        public UnauthDocValidationControl(string ip, IFormFile file) : base(file) {
            _userDataHandler = new UnauthUserApplicationDataHandler();
            _userData = _userDataHandler.GetUserData(ip);
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
            _userDataHandler.SetUserData(_userData);
        }
    }
}
