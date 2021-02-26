using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.Model.UserPayment;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;
using System.Collections.Generic;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.Model.Request;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SetPremiumController : ControllerBase {
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
                RequestLogger.Add(new RequestStatusLog(RequestTypes.SetPremium, logInfo, RequestStatuses.Start));

                string uId = _googleApi.GetUserId(payment.AccessToken);
                _userDataHandle = new UserDataHandle(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist()) {
                    throw new UserDoesNotExistException();
                }

                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.InAppItemId, payment.PurchaseToken);

                _setUserData.SetPremium();

                RequestLogger.Add(new RequestStatusLog(RequestTypes.SetPremium, logInfo, RequestStatuses.Completed));

                return Ok("success");
            } catch (Exception exception) {
                RequestLogger.Add(new RequestExceptionLog(RequestTypes.SetPremium, logInfo, exception.Message));

                return new ObjectResult(exception.Message) {
                    StatusCode = 500
                };
            }
        }
    }
}
