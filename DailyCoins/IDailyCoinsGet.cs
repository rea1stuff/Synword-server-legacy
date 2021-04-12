namespace SynWord_Server_CSharp.DailyCoins {
    public interface IDailyCoinsGet {
        bool Is24HoursPassed();
        void GiveCoinsToUser();
    }
}
