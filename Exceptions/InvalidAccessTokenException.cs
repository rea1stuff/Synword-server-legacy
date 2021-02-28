using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class InvalidAccessTokenException : Exception {
        const string message = "Invalid Access Token";
        public InvalidAccessTokenException() : base(message) { }
    }
}
