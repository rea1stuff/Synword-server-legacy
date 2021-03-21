namespace SynWord_Server_CSharp.Exceptions {
    public class NotEnoughCoinsException : UserException {
        const string message = "notEnoughCoins";

        public NotEnoughCoinsException() : base(message) {}
    }
}
