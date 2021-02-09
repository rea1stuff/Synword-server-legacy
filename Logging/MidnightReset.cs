using System;
using System.Threading;
using System.Configuration;

namespace SynWord_Server_CSharp.Logging {
    public class MidnightReset {
        public void UseReset() {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            if (configuration.AppSettings.Settings["lastLogResetDate"].Value == "") {
                ChangeLastLogResetDate();
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            DateTime lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));

            lastLogResetDate = DateTime.Now.AddDays(-1.5);

            if ((DateTime.Now - lastLogResetDate).TotalDays >= 1) {
                Reset();
                ChangeLastLogResetDate();
                lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));
            }

            while (true) {
                double milliseconds = (lastLogResetDate.AddDays(1) - DateTime.Now).TotalMilliseconds;
                Thread.Sleep((int)milliseconds);
                Reset();
                ChangeLastLogResetDate();
                lastLogResetDate = DateTime.Parse(string.Format(configuration.AppSettings.Settings["lastLogResetDate"].Value));
            }
        }

        private void Reset() {
            new FreeSynonimizerUsageLog().ResetNumberOfUsesIn24Hours();
            new UniqueCheckUsageLog().ResetNumberOfUsesIn24Hours();
            new VisitsLog().ResetNumberOfUsesIn24Hours();
        }

        private void ChangeLastLogResetDate() {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["lastLogResetDate"].Value = DateTime.Parse(string.Format("00:00:00")).ToString();
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
