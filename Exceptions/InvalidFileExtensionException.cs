namespace SynWord_Server_CSharp.Exceptions {
    public class InvalidFileExtensionException : UserException {
        const string message = "invalidFileExtension";

        public InvalidFileExtensionException() : base(message) {}
    }
}
