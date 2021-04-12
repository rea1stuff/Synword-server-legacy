using Microsoft.AspNetCore.Mvc;
using System;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.GoogleApi;
using SynWord_Server_CSharp.UserDataHandlers;
using SynWord_Server_CSharp.Model.UserData;
using SynWord_Server_CSharp.DAO;
using SynWord_Server_CSharp.Model.UserPayment;

namespace SynWord_Server_CSharp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseVerificationController : ControllerBase {
        UserApplicationDataDao _userDao = new UserApplicationDataDao();
        PaymentsDao _paymentsDao = new PaymentsDao();
        UserApplicationDataHandler _userDataHandler = new UserApplicationDataHandler();
        [HttpPost]
        public IActionResult Post([FromForm] RequestPaymentModel purchase) {
            try {
                Console.WriteLine("Verification");
                Console.WriteLine(purchase.PurchaseToken);
                Console.WriteLine(purchase.ProductId);

                UserApplicationDataModel userData = _userDao.GetUserDataById(purchase.Uid);

                int coins = userData.coins;

                UserPaymentHandler.PaymentVerify(purchase.ProductId, purchase.PurchaseToken);

                PaymentModel paymentModel = new PaymentModel();
                Order order = new Order();

                order.productId = purchase.ProductId;
                order.purchaseToken = purchase.PurchaseToken;

                paymentModel.uId = purchase.Uid;
                paymentModel.orders.Add(order);

                if (!_paymentsDao.IsUserExist(purchase.Uid)) {
                    _paymentsDao.Create(paymentModel);
                } else {
                    PaymentModel user = _paymentsDao.GetUserDataById(purchase.Uid);
                    foreach (var orders in user.orders) {
                        if (orders.purchaseToken == purchase.PurchaseToken) {
                            throw new OrderHasAlreadyCompletedException();
                        }
                    }
                    _paymentsDao.SetUserData(paymentModel);
                }

                switch (purchase.ProductId) {
                    case "premium": _userDataHandler.SetPremium(userData.uId); break;
                    case "100_coins": {
                            userData.coins += 100;
                            _userDao.SetUserData(userData);
                                } break;
                    case "300_coins": {
                            userData.coins += 300;
                            _userDao.SetUserData(userData);
                        }
                        break;
                    case "600_coins": {
                            userData.coins += 600;
                            _userDao.SetUserData(userData);
                        }
                        break;
                    case "1000_coins": {
                            userData.coins += 1000;
                            _userDao.SetUserData(userData);
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
            catch(OrderHasAlreadyCompletedException ex) {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}