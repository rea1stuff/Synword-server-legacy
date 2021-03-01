namespace SynWord_Server_CSharp.Constants {
    public class UserLimits {
        public const int UniqueCheckRequests = 7;
        public const int UniqueUpRequests = 100;
        public const int DocumentUniqueUpRequests = 30;
        public const int UniqueCheckMaxSymbolLimit = 20000;
        public const int UniqueUpMaxSymbolLimit = 20000;
        public const int DocumentMaxSymbolLimit = 40000;
        public const int DocumentUniqueCheckMinSymbolLimit = 5000;
    }
}
