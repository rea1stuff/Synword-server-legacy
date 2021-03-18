using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions {
    public class NoPremiumException : UserException {
        const string message = "noPremium";

        public NoPremiumException() : base(message) { }
    }
}
