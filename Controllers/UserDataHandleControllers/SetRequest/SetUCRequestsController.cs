using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Model.UserPayment;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers.SetRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetUCRequestsController : ControllerBase
    {
        SetUserData _setUserData;
        GetUserData _getUserData;

        [HttpPost]
        public ActionResult Post([FromBody] UserPaymentModel payment)
        {
            _getUserData = new GetUserData(payment.uId);
            _setUserData = new SetUserData(payment.uId);
            try
            {
                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.inAppItemId, payment.purchaseToken);

                int count = 10;
                _setUserData.SetUniqueCheckRequest(count);
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
