using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions
{
    public class InvalidTokenException : Exception
    {
        const string message = "invalidToken";
        public InvalidTokenException() : base(message)
        {

        }
    }
}
