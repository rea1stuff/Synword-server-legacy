using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;
using SynWord_Server_CSharp.Model.UserPayment;
using SynWord_Server_CSharp.UserData;
using System;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers.SetSymbolLimit {
    [Route("api/[controller]")]
    [ApiController]
    public class SetUUSymbolLimitController : ControllerBase {
        SetUserData _setUserData;
        UserDataHandle _userDataHandle;
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        [HttpPost]
        public ActionResult Post([FromBody] UserPaymentModel payment) {
            string clientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Dictionary<string, dynamic> logInfo = new Dictionary<string, dynamic> {
                { "Ip", clientIp },
                { "AccessToken", payment.AccessToken },
                { "InAppItemId", payment.InAppItemId },
                { "PurchaseToken", payment.PurchaseToken }
            };

            try {
                RequestLogger.LogRequestStatus(RequestTypes.SetUUSymbolLimit, logInfo, RequestStatuses.Start);

                string uId = _googleApi.GetUserId(payment.AccessToken);
                _setUserData = new SetUserData(uId);
                _userDataHandle = new UserDataHandle(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }

                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.InAppItemId, payment.PurchaseToken);

                int count = 20000;
                _setUserData.SetUniqueUpMaxSymbolLimit(count);

                RequestLogger.LogRequestStatus(RequestTypes.SetUUSymbolLimit, logInfo, RequestStatuses.Completed);

                return Ok("success");
            } catch (Exception exception) {
                RequestLogger.LogException(RequestTypes.SetUUSymbolLimit, logInfo, exception.Message);

                return BadRequest(exception.Message);
            }
        }
    }
}
