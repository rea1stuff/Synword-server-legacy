namespace SynWord_Server_CSharp.Model.Request {
    public class RequestType {
        public string Name { get; }

        public RequestType(string name) {
            Name = name;
        }

        public static bool operator ==(RequestType first, RequestType second) {
            if (first.Name == second.Name) {
                return true;
            } else {
                return false;
            }
        }

        public static bool operator !=(RequestType first, RequestType second) {
            if (first.Name != second.Name) {
                return true;
            } else {
                return false;
            }
        }
    }
}
