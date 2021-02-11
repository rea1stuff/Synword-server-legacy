using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.UserData
{
    public class UserLimits
    {
        public static int UniqueCheckRequests { get; } = 10;
        public static int UniqueUpRequests { get; } = 1000;
        public static int DocumentUniqueUpRequests { get; } = 30;
        public static int UniqueCheckMaxSymbolLimit { get; } = 20000;
        public static int UniqueUpMaxSymbolLimit { get; } = 20000;
        public static int DocumentMaxSymbolLimit { get; } = 40000;
        public static int DocumentUniqueCheckMinSymbolLimit { get; } = 5000;
    }
}
