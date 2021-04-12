using System;

namespace SynWord_Server_CSharp.Exceptions.Dao {
    public class UserApplicationDataDaoException : Exception {
        public UserApplicationDataDaoException(string message) : base(message) { }
    }
}
