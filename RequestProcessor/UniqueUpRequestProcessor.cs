using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.RequestProcessor.RequestValidators;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Log;
using SynWord_Server_CSharp.Model.Request;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.RequestProcessor.RequestHandlers;
using SynWord_Server_CSharp.Exceptions;

namespace SynWord_Server_CSharp.RequestProcessor {
    public class UniqueUpRequestProcessor {
        IValidationControl _validationControl;
        IRequestHandler _uniqueUp = new UniqueUpRequestHandler();
        int _requestPrice = RequestPrices.UniqueUpPrice;

        public async Task<IActionResult> UnauthUserRequestExecution(IUserLogDataModel user) {
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, user.ToDictionary(), RequestStatuses.Start));

                int textLength = user.UserModel.Text.Length;

                _validationControl = new UnauthValidationControl(user.UserModel.Uid);

                _validationControl.MinSymbolLimitVerification(textLength);
                _validationControl.UniqueUpMaxSymbolLimitVerification(textLength);
                _validationControl.IsUserHaveEnoughCoins(_requestPrice);

                IActionResult result = await _uniqueUp.HandleRequest(user.UserModel.Text);

                _validationControl.SpendCoins(_requestPrice);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, user.ToDictionary(), RequestStatuses.Completed));

                return result;
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueUp, user.ToDictionary(), exception.Message));
                return RequestExceptionHandler.Handle(exception);
            }
        }

        public async Task<IActionResult> AuthUserRequestExecution(IUserLogDataModel user) {
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, user.ToDictionary(), RequestStatuses.Start));

                int textLength = user.UserModel.Text.Length;

                _validationControl = new AuthValidationControl(user.UserModel.Uid);

                _validationControl.MinSymbolLimitVerification(textLength);
                _validationControl.UniqueUpMaxSymbolLimitVerification(textLength);
                _validationControl.IsUserHaveEnoughCoins(_requestPrice);

                IActionResult result = await _uniqueUp.HandleRequest(user.UserModel.Text);

                _validationControl.SpendCoins(_requestPrice);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.UniqueUp, user.ToDictionary(), RequestStatuses.Completed));

                return result;
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.UniqueUp, user.ToDictionary(), exception.Message));
                return RequestExceptionHandler.Handle(exception);
            }
        }
    }
}
