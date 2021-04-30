using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Log {
    public class UnauthUserLogDataModel : IUserLogDataModel {
        public UnauthUserLogDataModel(string ip, IUserRequestModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }

        public override string Ip { get; }

        public override IUserRequestModel UserModel { get; set; }

        public override Dictionary<string, dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", UserModel.Uid },
                { "TextLength", UserModel.Text.Length }
            };
        }
    }
}
