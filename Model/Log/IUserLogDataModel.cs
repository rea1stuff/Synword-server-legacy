using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model.Log {
    public abstract class IUserLogDataModel {
        public abstract string Ip { get; }
        public abstract IUserModel UserModel { get; set; }
        public abstract Dictionary<string, dynamic> ToDictionary();
    }
}
