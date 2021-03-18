using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.DAO {
    interface IDao<T> {
        public void Create(T userData);
        List<T> GetAllUsersData();
        public T GetUserDataById(string uId);
        public void SetUserData(T userData);
        public void DeleteUserData(string uId);
    }
}
