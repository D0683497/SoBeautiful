using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SoBeautiful.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<SeedData>>();
                
                #region Database

                logger.LogInformation("開始創建資料庫");
                var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                var db = applicationDbContext.Database.EnsureCreated();
                logger.LogInformation("創資料庫完成");

                #endregion
                
                #region Data
                
                if (db)
                {
                    // logger.LogInformation("開始初始化資料");
                    // InitData(services, logger);
                    // logger.LogInformation("初始化資料完成");
                }
                else
                {
                    logger.LogInformation("資料庫已有資料");
                }
                
                #endregion
            }
        }
    }
}