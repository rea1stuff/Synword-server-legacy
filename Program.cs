using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.Logging;
using SynWord_Server_CSharp.DailyCoins;
using System.Net;
using Microsoft.AspNetCore;
using System.Configuration;
using System.IO;

namespace SynWord_Server_CSharp {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
                    WebHost.CreateDefaultBuilder(args)
                        .UseKestrel(options => {
                            SynonymDictionary.InitializeDictionary();
                            Task.Run(() => new MidnightReset().UseReset());
                            Task.Run(() => RequestLogger.Logging());

                            options.Listen(IPAddress.Any, 5000);
                            options.Listen(IPAddress.Any, 5001, listenOptions => {
                                listenOptions.UseHttps(Directory.GetCurrentDirectory() + "/Files/certificate.pfx", ConfigurationManager.AppSettings["sslCertPass"]);
                            });
                        })
                        .UseStartup<Startup>();

    }
}
