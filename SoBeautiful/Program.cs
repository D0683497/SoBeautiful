using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SoBeautiful.Data;

namespace SoBeautiful
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            SeedData.EnsureSeedData(host);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
