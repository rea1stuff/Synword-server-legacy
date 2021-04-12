using SynWord_Server_CSharp.Model.FileUpload;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Log.Documents {
    public class UnauthDocUniqueUpLogDataModel {
        public UnauthDocUniqueUpLogDataModel(string ip, FileUploadModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }
        public string Ip { get; set; }
        public FileUploadModel UserModel { get; set; }
        public Dictionary<string, dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", UserModel.Uid }
            };
        }
    }
}
