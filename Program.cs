using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SynWord_Server_CSharp.Synonymize;
using SynWord_Server_CSharp.Logging;

namespace SynWord_Server_CSharp {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    SynonymDictionary.InitializeDictionary();
                    Task.Run(() => new MidnightReset().UseReset());
                    Task.Run(() => RequestLogger.Logging());
                });
    }
}
