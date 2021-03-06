using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class DailyLimitReachedException : Exception
    {
        const string message = "dailyLimitReached";

        public DailyLimitReachedException() : base(message) {}
    }
}
