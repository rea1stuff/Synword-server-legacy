namespace SynWord_Server_CSharp.Exceptions {
    public class NoPremiumException : UserException {
        const string message = "noPremium";

        public NoPremiumException() : base(message) {}
    }
}
