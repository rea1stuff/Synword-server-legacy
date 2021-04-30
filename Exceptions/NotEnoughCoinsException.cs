using System.Collections;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Exceptions {
    public class NotEnoughCoinsException : UserException {
        const string message = "notEnoughCoins";
        public override IDictionary Data {
            get {
                return new Dictionary<string, dynamic> {
                    { "message", message },
                };
            }
        }

        public NotEnoughCoinsException() : base(message) {}
    }
}
