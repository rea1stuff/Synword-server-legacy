using System.Collections;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Exceptions.SymbolLimit {
    public class MinSymbolLimitException : UserException {
        const string message = "minSymbolLimit";
        int symbolCount;
        public override IDictionary Data {
            get {
                return new Dictionary<string, dynamic> {
                    { "message", message },
                    { "symbolCount", symbolCount },
                };
            }
        }
        public MinSymbolLimitException(int symbolCount) : base(message) {
            this.symbolCount = symbolCount;
        }
    }
}
