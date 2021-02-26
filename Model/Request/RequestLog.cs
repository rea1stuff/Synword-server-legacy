using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Request {
    public class RequestLog {
        public RequestType Type { get; }
        public Dictionary<string, dynamic> OtherInfo { get; }

        public RequestLog(RequestType type, Dictionary<string, dynamic> otherInfo) {
            Type = type;
            OtherInfo = otherInfo;
        }
    }
}
