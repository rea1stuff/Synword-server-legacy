using SynWord_Server_CSharp.Model.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model.Log.Documents {
    public class AuthDocUniqueUpLogDataModel {
        public AuthDocUniqueUpLogDataModel(string ip, AuthFileUploadModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }
        public AuthFileUploadModel UserModel { get; set; }
        public string Ip { get; set; }
        public Dictionary<string, dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", UserModel.Uid }
            };
        }
    }
}
