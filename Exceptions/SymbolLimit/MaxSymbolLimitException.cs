using System;

namespace SynWord_Server_CSharp.Exceptions.SymbolLimit
{
    public class MaxSymbolLimitException : UserException {
        const string message = "maxSymbolLimit";

        public MaxSymbolLimitException() : base(message) {}
    }
}
