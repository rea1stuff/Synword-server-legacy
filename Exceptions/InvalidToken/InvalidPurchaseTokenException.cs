using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions {
    public class InvalidPurchaseTokenException : UserException {
        const string message = "invalidPurchaseToken";
        public InvalidPurchaseTokenException() : base(message) {

        }
    }
}
