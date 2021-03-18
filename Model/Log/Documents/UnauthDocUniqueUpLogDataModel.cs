using SynWord_Server_CSharp.Model.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model.Log.Documents {
    public class UnauthDocUniqueUpLogDataModel {
        public UnauthDocUniqueUpLogDataModel(string ip, FileUploadModel userModel) {
            Ip = ip;
            UserModel = userModel;
        }
        public string Ip { get; set; }
        public string Uid { get; set; }
        public FileUploadModel UserModel { get; set; }
        public Dictionary<string, dynamic> ToDictionary() {
            return new Dictionary<string, dynamic> {
                { "Ip", Ip },
                { "Uid", Uid }
            };
        }
    }
}
