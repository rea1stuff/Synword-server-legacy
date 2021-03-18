using MongoDB.Bson.Serialization.Attributes;

namespace SynWord_Server_CSharp.Model.UserData {
    [BsonIgnoreExtraElements]
    public class UnauthUserApplicationDataModel {
        public string uId { get; set; }
        public string ip { get; set; }
        public int coins { get; set; }
        public int uniqueCheckMaxSymbolLimit { get; set; }
        public int uniqueUpMaxSymbolLimit { get; set; }
        public int documentUniqueUpMaxSymbolLimit { get; set; }
        public string lastVisitDate { get; set; }
        public string creationDate { get; set; }
    }
}
