using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Model.UserPayment;
using SynWord_Server_CSharp.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Controllers.UserDataHandleControllers.SetSymbolLimit
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetUCSymbolLimitController : ControllerBase
    {
        SetUserData _setUserData;

        [HttpPost]
        public ActionResult Post([FromBody] UserPaymentModel payment)
        {
            _setUserData = new SetUserData(payment.uId);
            try
            {
                UserPaymentCheck paymentCheck = new UserPaymentCheck();
                paymentCheck.PaymentCheck(payment.inAppItemId, payment.purchaseToken);

                int count = 20000;
                _setUserData.SetUniqueCheckMaxSymbolLimit(count);
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
