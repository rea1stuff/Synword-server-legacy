using System.Collections;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Exceptions.SymbolLimit {
    public class MaxSymbolLimitException : UserException {
        const string message = "maxSymbolLimit";
        int symbolCount;
        public override IDictionary Data {
            get {
                return new Dictionary<string, dynamic> {
                    { "message", message },
                    { "symbolCount", symbolCount }
                };
            } 
        }
        public MaxSymbolLimitException(int symbolCount) : base(message) {
            this.symbolCount = symbolCount;
        }
    }
}
