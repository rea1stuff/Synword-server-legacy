using System;

namespace SynWord_Server_CSharp.Exceptions.SymbolLimit {
    public class MinSymbolLimitException : UserException {
        const string message = "minSymbolLimit";

        public MinSymbolLimitException() : base(message) { }
    }
}
