using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions {
    public class NotEnoughCoinsException : UserException {
        const string message = "notEnoughCoins";

        public NotEnoughCoinsException() : base(message) { }
    }
}
