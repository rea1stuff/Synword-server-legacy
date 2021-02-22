using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        const string message = "userDoesNotExist";
        public UserDoesNotExistException() : base(message)
        {

        }
    }
}
