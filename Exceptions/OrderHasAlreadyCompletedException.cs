namespace SynWord_Server_CSharp.Exceptions {
    public class OrderHasAlreadyCompletedException : UserException {
        const string message = "orderHasAlreadyCompleted";

        public OrderHasAlreadyCompletedException() : base(message) { }
    }
}
