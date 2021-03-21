using System;

namespace SynWord_Server_CSharp.Exceptions {
    public class UserException : Exception {
        public UserException(string message) : base(message) {}
    }
}
