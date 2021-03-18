using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Exceptions.SymbolLimit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators {
    public abstract class IValidationControl {
        protected abstract int GetCoins();
        protected abstract int GetUniqueCheckMaxSymbolLimit();
        protected abstract int GetUniqueUpMaxSymbolLimit();
        public abstract void SpendCoins(int price);
        public void IsUserHaveEnoughCoins(int price) {
            if (GetCoins() < price) {
                throw new NotEnoughCoinsException();
            }
        }
        public void UniqueCheckMaxSymbolLimitVerification(int textLength) {
            if (GetUniqueCheckMaxSymbolLimit() < textLength) {
                throw new MaxSymbolLimitException();
            }
        }
        public void UniqueUpMaxSymbolLimitVerification(int textLength) {
            if (GetUniqueUpMaxSymbolLimit() < textLength) {
                throw new MaxSymbolLimitException();
            }
        }
        public void MinSymbolLimitVerification(int textLength) {
            if (textLength < UserLimits.MinSymbolLimit) {
                throw new MinSymbolLimitException();
            }
        }
    }
}
