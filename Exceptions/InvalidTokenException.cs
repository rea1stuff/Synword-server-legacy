using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class InvalidTokenException : Exception
    {
        const string message = "Invalid token";

        public InvalidTokenException() : base(message) {}
    }
}
