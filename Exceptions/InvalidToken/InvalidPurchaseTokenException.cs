namespace SynWord_Server_CSharp.Exceptions {
    public class InvalidPurchaseTokenException : UserException {
        const string message = "invalidPurchaseToken";

        public InvalidPurchaseTokenException() : base(message) {}
    }
}
