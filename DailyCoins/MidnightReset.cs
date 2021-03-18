using System;
using System.Threading;
using System.Configuration;

namespace SynWord_Server_CSharp.DailyCoins {
    public class MidnightReset {
        public void UseReset() {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            if (configuration.AppSettings.Settings["lastLogResetDate"].Value == "") {
                ChangeLastLogResetDate();
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            DateTime lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));

            if (Math.Abs((DateTime.Now - lastLogResetDate).TotalDays) >= 1) {
                Reset();
                ChangeLastLogResetDate();
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));
            }

            while (true) {
                double milliseconds = (lastLogResetDate.AddDays(1) - DateTime.Now).TotalMilliseconds;
                Thread.Sleep((int)milliseconds);
                Reset();
                ChangeLastLogResetDate();
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));
            }
        }

        private void Reset() {
            new UnauthUserCoinsReset().Reset();
        }

        private void ChangeLastLogResetDate() {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["lastLogResetDate"].Value = DateTime.Parse(string.Format("00:00:00")).ToString();
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
