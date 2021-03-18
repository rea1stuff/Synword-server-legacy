using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.UserDataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents {
    public class AuthDocValidationControl : IDocumentValidationControl {
        UserApplicationDataModel _userData;
        IUserDataHandler<UserApplicationDataModel> _userDataHandler;
        public AuthDocValidationControl(string uId, IFormFile file) : base(file) {
            _userDataHandler = new UserApplicationDataHandler();
            _userData = _userDataHandler.GetUserData(uId);
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
            _userDataHandler.SetUserData(_userData);
        }
    }
}
