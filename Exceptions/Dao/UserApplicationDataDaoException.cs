using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions.Dao {
    public class UserApplicationDataDaoException : Exception {
        public UserApplicationDataDaoException(string message) : base(message) { }
    }
}
