namespace SynWord_Server_CSharp.Exceptions {
    public class UserDoesNotExistException : UserException {
        const string message = "userDoesNotExist";

        public UserDoesNotExistException() : base(message) {}
    }
}
