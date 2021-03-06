using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Constants {
    public class PremiumUserLimits {
        public const int UniqueCheckRequests = 20;
        public const int UniqueUpRequests = 10000;
        public const int UniqueCheckMaxSymbolLimit = 40000;
        public const int UniqueUpMaxSymbolLimit = 40000;
        public const int DocumentMaxSymbolLimit = 80000;
    }
}
