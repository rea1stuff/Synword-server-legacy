using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SynWord_Server_CSharp.Model.UserPayment;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetPremiumController : ControllerBase
    {
        SetUserData _setUserData;
        UserDataHandle _userDataHandle;
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();

        [HttpPost]
        public ActionResult Post([FromBody] UserPaymentModel payment)
        {
            try
            {
                string uId = _googleApi.GetUserId(payment.accessToken);
                _userDataHandle = new UserDataHandle(uId);
                _setUserData = new SetUserData(uId);

                if (!_userDataHandle.IsUserExist())
                {
                    throw new UserDoesNotExistException();
                }
                
                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.inAppItemId, payment.purchaseToken);

                _setUserData.SetPremium();

                return Ok("success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetPremium Exception: " + ex.Message);
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
