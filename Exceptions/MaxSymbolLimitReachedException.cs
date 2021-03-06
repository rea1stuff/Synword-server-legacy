using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class MaxSymbolLimitReachedException : Exception
    {
        const string message = "maxSymbolLimitReached";

        public MaxSymbolLimitReachedException() : base(message) {}
    }
}
