using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Request {
    public class RequestExceptionLog : RequestLog {
        public string ExceptionMessage { get; }

        public RequestExceptionLog(RequestType type, Dictionary<string, dynamic> otherInfo, string message) : base(type, otherInfo) {
            ExceptionMessage = message;
        }
    }
}
