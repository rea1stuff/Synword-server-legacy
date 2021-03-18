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
using SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents;
using SynWord_Server_CSharp.Model.Log.Documents;
using SynWord_Server_CSharp.RequestProcessor.RequestHandlers.Documents;
using SynWord_Server_CSharp.DocumentHandling.Docx;

namespace SynWord_Server_CSharp.RequestProcessor {
    public class DocxUniqueCheckRequestProcessor {
        IDocumentValidationControl _validationControl;
        IDocumentRequestHandler _uniqueCheck;
        int _requestPrice = RequestPrices.DocumentUniqueCheckPrice;
        string _filePath;
        public DocxUniqueCheckRequestProcessor(string filePath) {
            _filePath = filePath;
            _uniqueCheck = new UniqueCheckDocRequestHandler(filePath);
        }
        public async Task<IActionResult> AuthUserRequestExecution(DocUniqueCheckLogDataModel user) {
            try {
                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueCheck, user.ToDictionary(), RequestStatuses.Start));

                _validationControl = new AuthDocValidationControl(user.Ip, user.UserModel.Files);

                _validationControl.PremiumVerification();

                _validationControl.FileExtensionVerification();
                _validationControl.IsDirectoryExists();
                _validationControl.CreateFile(_filePath, user.UserModel.Files);

                int symbolCount = DocxGet.GetSymbolCount(_filePath);

                _validationControl.MinSymbolLimitVerification(symbolCount);
                _validationControl.UniqueCheckMaxSymbolLimitVerification(symbolCount);
                _validationControl.IsUserHaveEnoughCoins(_requestPrice);

                IActionResult result = await _uniqueCheck.HandleRequest();

                _validationControl.SpendCoins(_requestPrice);

                RequestLogger.Add(new RequestStatusLog(RequestTypes.DocxUniqueCheck, user.ToDictionary(), RequestStatuses.Completed));

                return result;
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.DocxUniqueCheck, user.ToDictionary(), exception.Message));
                return RequestExceptionHandler.Handle(exception);
            }
        }
    }
}
