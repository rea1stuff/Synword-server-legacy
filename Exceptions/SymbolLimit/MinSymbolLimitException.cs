using System.Collections;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Exceptions.SymbolLimit {
    public class MinSymbolLimitException : UserException {
        const string message = "minSymbolLimit";
        int symbolCount;
        int symbolLimit;
        public override IDictionary Data {
            get {
                return new Dictionary<string, dynamic> {
                    { "message", message },
                    { "symbolCount", symbolCount },
                    { "symbolLimit", symbolLimit }
                };
            }
        }
        public MinSymbolLimitException(int symbolCount, int symbolLimit) : base(message) {
            this.symbolCount = symbolCount;
            this.symbolLimit = symbolLimit;
        }
    }
}
