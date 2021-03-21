namespace SynWord_Server_CSharp.Exceptions {
    public class DailyLimitReachedException : UserException {
        const string message = "dailyLimitReached";

        public DailyLimitReachedException() : base(message) {}
    }
}
