using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.DailyCoins {
    public interface IDailyCoinsGet {
        bool Is24HoursPassed();
        void GiveCoinsToUser();
    }
}
