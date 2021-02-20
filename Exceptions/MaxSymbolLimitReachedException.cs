using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions
{
    public class MaxSymbolLimitReachedException : Exception
    {
        const string message = "maxSymbolLimitReached";
        public MaxSymbolLimitReachedException() : base(message)
        {

        }
    }
}
