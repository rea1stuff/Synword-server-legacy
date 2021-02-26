using System;

namespace SynWord_Server_CSharp.Exceptions
{
    public class DailyLimitReachedException : Exception
    {
        const string message = "Daily limit reached";

        public DailyLimitReachedException() : base(message) {}
    }
}
