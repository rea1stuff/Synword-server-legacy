using System.Collections;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Exceptions {
    public class NotEnoughCoinsException : UserException {
        const string message = "notEnoughCoins";
        int balance;
        int price;
        public override IDictionary Data {
            get {
                return new Dictionary<string, dynamic> {
                    { "message", message },
                    { "balance", balance },
                    { "price", price }
                };
            }
        }

        public NotEnoughCoinsException(int balance, int price) : base(message) {
            this.balance = balance;
            this.price = price;
        }
    }
}
