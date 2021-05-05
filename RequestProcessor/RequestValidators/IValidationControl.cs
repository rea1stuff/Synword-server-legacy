using SynWord_Server_CSharp.Constants;
using SynWord_Server_CSharp.Exceptions;
using SynWord_Server_CSharp.Exceptions.SymbolLimit;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators {
    public abstract class IValidationControl {
        protected abstract int GetCoins();
        protected abstract int GetUniqueCheckMaxSymbolLimit();
        protected abstract int GetUniqueUpMaxSymbolLimit();
        public abstract void SpendCoins(int price);

        public void IsUserHaveEnoughCoins(int price) {
            int balance = GetCoins();
            if (balance < price) {
                throw new NotEnoughCoinsException(balance, price);
            }
        }

        public void UniqueCheckMaxSymbolLimitVerification(int textLength) {
            int maxSymbolLimit = GetUniqueCheckMaxSymbolLimit();
            if (maxSymbolLimit < textLength) {
                throw new MaxSymbolLimitException(textLength, maxSymbolLimit);
            }
        }

        public void UniqueUpMaxSymbolLimitVerification(int textLength) {
            int maxSymbolLimit = GetUniqueUpMaxSymbolLimit();
            if (maxSymbolLimit < textLength) {
                throw new MaxSymbolLimitException(textLength, maxSymbolLimit);
            }
        }

        public void MinSymbolLimitVerification(int textLength) {
            if (textLength < UserLimits.MinSymbolLimit) {
                throw new MinSymbolLimitException(textLength, UserLimits.MinSymbolLimit);
            }
        }
    }
}
