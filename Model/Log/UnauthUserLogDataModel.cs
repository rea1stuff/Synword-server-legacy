using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model;

namespace SynWord_Server_CSharp.Model.Log {
    public class UnauthUserLogDataModel : IUserLogDataModel {
        public UnauthUserLogDataModel(string ip, IUserModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }
        public override string Ip { get; }
        public override IUserModel UserModel { get; set; }
        public override Dictionary<string, dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", UserModel.Uid },
                { "TextLength", UserModel.Text.Length }
            };
        }
    }
}
