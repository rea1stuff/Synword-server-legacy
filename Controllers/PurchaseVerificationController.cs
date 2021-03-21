using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.Purchase;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.Model.UserData;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseVerificationController : ControllerBase {
        UserApplicationDataHandler _dataHandler = new UserApplicationDataHandler();

        [HttpPost]
        public IActionResult Post([FromBody] PurchaseModel purchase) {
            try {
                Console.WriteLine("Verification");
                Console.WriteLine(purchase.Uid);
                Console.WriteLine(purchase.ProductId);
                Console.WriteLine(purchase.PurchaseToken);

                UserApplicationDataModel userData = _dataHandler.GetUserData(purchase.Uid);

                int coins = userData.coins;

                UserPaymentHandler.PaymentVerify(purchase.ProductId, purchase.PurchaseToken);

                switch (purchase.ProductId) {
                    case "premium": _dataHandler.SetPremium(userData.uId); break;
                    case "100_coins": {
                            userData.coins += 100;
                            _dataHandler.SetUserData(userData);
                                } break;
                    case "300_coins": {
                            userData.coins += 300;
                            _dataHandler.SetUserData(userData);
                        }
                        break;
                    case "600_coins": {
                            userData.coins += 600;
                            _dataHandler.SetUserData(userData);
                        }
                        break;
                    case "1000_coins": {
                            userData.coins += 1000;
                            _dataHandler.SetUserData(userData);
                        }
                        break;
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