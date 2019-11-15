using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace ARngryBirdsBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartServer();
        }

        private static void StartServer()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
#if DEBUG
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                    logging.AddConsole();
                })
#endif                           
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}