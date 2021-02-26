using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.Request {
    public class RequestStatusLog : RequestLog {
        public RequestStatus RequestStatus { get; }

        public RequestStatusLog(RequestType type, Dictionary<string, dynamic> otherInfo, RequestStatus status) : base(type, otherInfo) {
            RequestStatus = status;
        }
    }
}
