using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Log {
    public class AuthUserLogDataModel : IUserLogDataModel {
        public AuthUserLogDataModel(string ip, IUserModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }
        public override string Ip { get; }
        public override IUserModel UserModel { get; set; }
        public override Dictionary<string,dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", UserModel.Uid },
                { "TextLength", UserModel.Text.Length }
            };
        }
    }
}
