using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.UserDataHandlers {
    public interface IUserDataHandler <T> {
        public void CreateUser(string uId);
        public T GetUserData(string uId);
        public void SetUserData(T userData);
        public void DeleteUserData(string uId);
        public bool IsUserExist(string uId);
    }
}
