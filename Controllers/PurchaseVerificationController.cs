using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.Purchase;
using SynWord_Server_CSharp.UserData;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseVerificationController : ControllerBase {
        UserPaymentCheck paymentCheck = new UserPaymentCheck();
        GoogleOauth2Api _googleApi = new GoogleOauth2Api();
        SetUserData _setUserData;
        GetUserData _getUserData;

        [HttpPost]
        public IActionResult Post([FromBody] PurchaseModel purchase) {
            try {
                Console.WriteLine("Verification");
                Console.WriteLine(purchase.AccessToken);
                Console.WriteLine(purchase.ProductId);
                Console.WriteLine(purchase.PurchaseToken);
                string uId = _googleApi.GetUserId(purchase.AccessToken);
                _getUserData = new GetUserData(uId);
                _setUserData = new SetUserData(uId);

                int uniqueCheckRequests = _getUserData.GetUniqueCheckRequests();

                paymentCheck.PaymentCheck(purchase.ProductId, purchase.PurchaseToken);

                switch (purchase.ProductId) {
                    case "premium": _setUserData.SetPremium(); break;
                    case "100_plagiarism_check": _setUserData.SetUniqueCheckRequest(uniqueCheckRequests + 100); break;
                    case "300_plagiarism_check": _setUserData.SetUniqueCheckRequest(uniqueCheckRequests + 300); break;
                    case "600_plagiarism_check": _setUserData.SetUniqueCheckRequest(uniqueCheckRequests + 600); break;
                    case "1000_plagiarism_check": _setUserData.SetUniqueCheckRequest(uniqueCheckRequests + 1000); break;
                    default:
                        break;
                }
                Console.WriteLine("Completed");
                return Ok();
            } catch (InvalidPurchaseTokenException ex) {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidAccessTokenException ex) {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}