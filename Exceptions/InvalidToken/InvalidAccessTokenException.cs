using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class InvalidAccessTokenException : UserException {
        const string message = "invalidAccessToken";
        public InvalidAccessTokenException() : base(message) { }
    }
}
