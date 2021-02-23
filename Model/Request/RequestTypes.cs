namespace SynWord_Server_CSharp.Model.Request {
    static public class RequestTypes {
        static public RequestType UniqueUp { get; }
        static public RequestType UniqueUpAuth { get; }
        static public RequestType UniqueCheck { get; }
        static public RequestType UniqueCheckAuth { get; }
        static public RequestType DocxUniqueUp { get; }
        static public RequestType DocxUniqueUpAuth { get; }
        static public RequestType DocxUniqueCheck { get; }
        static public RequestType SetDocUU { get; }
        static public RequestType SetUCSymbolLimit { get; }
        static public RequestType SetUUSymbolLimit { get; }
        static public RequestType SetPremium { get; }
        static public RequestType Authorization { get; }
        static public RequestType Session { get; }

        static RequestTypes() {
            UniqueUp = new RequestType("UniqueUp");
            UniqueUpAuth = new RequestType("UniqueUpAuth");
            UniqueCheck = new RequestType("UniqueCheck");
            UniqueCheckAuth = new RequestType("UniqueCheckAuth");
            DocxUniqueUp = new RequestType("DocxUniqueUp");
            DocxUniqueUpAuth = new RequestType("DocxUniqueUpAuth");
            DocxUniqueCheck = new RequestType("DocxUniqueCheck");
            SetDocUU = new RequestType("SetDocUU");
            SetUCSymbolLimit = new RequestType("SetUcSymbolLimit");
            SetUUSymbolLimit = new RequestType("SetUUSymbolLimit");
            SetPremium = new RequestType("SetPremium");
            Authorization = new RequestType("Authorization");
            Session = new RequestType("Session");
        }
    }
}
