namespace SynWord_Server_CSharp.Model.Request {
    static public class RequestStatuses {
        static public RequestStatus Start { get; }
        static public RequestStatus Completed { get; }

        static RequestStatuses() {
            Start = new RequestStatus("Start");
            Completed = new RequestStatus("Completed");
        }
    }
}
