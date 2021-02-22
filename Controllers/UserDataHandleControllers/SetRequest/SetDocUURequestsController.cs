using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.Model.UserPayment;
using SynWord_Server_CSharp.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers.SetRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetDocUURequestsController : ControllerBase
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

                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.inAppItemId, payment.purchaseToken);

                int count = 10;
                _setUserData.SetDocumentUniqueUpRequests(count);
                return Ok("success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
