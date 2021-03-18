using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Exceptions.SymbolLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.Constants;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators {
    public class UnauthValidationControl : IValidationControl {
        UnauthUserApplicationDataModel _userData;
        IUserDataHandler<UnauthUserApplicationDataModel> _userDataHandler;
        public UnauthValidationControl(string uId) {
            _userDataHandler = new UnauthUserApplicationDataHandler();
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
