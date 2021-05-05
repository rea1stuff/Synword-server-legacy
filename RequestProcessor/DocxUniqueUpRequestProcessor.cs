using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;
using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.RequestProcessor.RequestHandlers;
using SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents;
using SynWord_Server_CSharp.Model.Log.Documents;
using SynWord_Server_CSharp.RequestProcessor.RequestHandlers.Documents;
using SynWord_Server_CSharp.DocumentHandling.Docx;

namespace SynWord_Server_CSharp.RequestProcessor {
    public class DocxUniqueUpRequestProcessor {
        private IDocumentValidationControl _validationControl;
        private UniqueUpDocRequestHandler _uniqueUp;
        private int _requestPrice = RequestPrices.DocumentUniqueUpPrice;
        private string _filePath;

        public DocxUniqueUpRequestProcessor(string filePath) {
            _filePath = filePath;
            _uniqueUp = new UniqueUpDocRequestHandler(filePath);
        }

        public async Task<IActionResult> UnauthUserRequestExecution(UnauthDocUniqueUpLogDataModel user) {
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), RequestStatuses.Start));

                _validationControl = new UnauthDocValidationControl(user.UserModel.Uid, user.UserModel.Files);

                _validationControl.FileExtensionVerification();
                _validationControl.IsDirectoryExists();
                _validationControl.CreateFile(_filePath, user.UserModel.Files);

                int symbolCount = DocxGet.GetSymbolCount(_filePath);

                _validationControl.MinSymbolLimitVerification(symbolCount);
                _validationControl.UniqueUpMaxSymbolLimitVerification(symbolCount);
                _validationControl.IsUserHaveEnoughCoins(_requestPrice);

                IActionResult result = await _uniqueUp.HandleRequest(user.UserModel.Language);

                _validationControl.SpendCoins(_requestPrice);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), RequestStatuses.Completed));

                return result;
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), exception.Message));
                return RequestExceptionHandler.Handle(exception);
            }
        }

        public async Task<IActionResult> AuthUserRequestExecution(AuthDocUniqueUpLogDataModel user) {
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), RequestStatuses.Start));

                _validationControl = new AuthDocValidationControl(user.UserModel.Uid, user.UserModel.Files);

                _validationControl.FileExtensionVerification();
                _validationControl.IsDirectoryExists();
                _validationControl.CreateFile(_filePath, user.UserModel.Files);

                int symbolCount = DocxGet.GetSymbolCount(_filePath);

                _validationControl.MinSymbolLimitVerification(symbolCount);
                _validationControl.UniqueUpMaxSymbolLimitVerification(symbolCount);
                _validationControl.IsUserHaveEnoughCoins(_requestPrice);

                IActionResult result = await _uniqueUp.HandleRequest(user.UserModel.Language);

                _validationControl.SpendCoins(_requestPrice);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), RequestStatuses.Completed));

                return result;
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.DocxUniqueUp, user.ToDictionary(), exception.Message));
                return RequestExceptionHandler.Handle(exception);
            }
        }
    }
}
