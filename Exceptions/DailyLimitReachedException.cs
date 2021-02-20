using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions
{
    public class DailyLimitReachedException : Exception
    {
        const string message = "dailyLimitReached";
        public DailyLimitReachedException() : base(message)
        {

        }
    }
}
