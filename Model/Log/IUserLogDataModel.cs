using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Log {
    public abstract class IUserLogDataModel {
        public abstract string Ip { get; }
        public abstract IUserRequestModel UserModel { get; set; }
        public abstract Dictionary<string, dynamic> ToDictionary();
    }
}
