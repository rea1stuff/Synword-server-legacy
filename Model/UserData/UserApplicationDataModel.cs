using MongoDB.Bson.Serialization.Attributes;

namespace SynWord_Server_CSharp.Model.UserData {
    [BsonIgnoreExtraElements]
    public class UserApplicationDataModel {
        public string uId { get; set; }
        public bool isPremium { get; set; }
        public int coins { get; set; }
        public int uniqueCheckMaxSymbolLimit { get; set; }
        public int uniqueUpMaxSymbolLimit { get; set; }
        public int documentUniqueCheckMaxSymbolLimit { get; set; }
        public int documentUniqueUpMaxSymbolLimit { get; set; }
        public string lastVisitDate { get; set; }
        public string creationDate { get; set; }
    }
}
