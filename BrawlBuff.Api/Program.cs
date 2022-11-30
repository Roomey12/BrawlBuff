using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Infrastructure.Extensions;
using BrawlBuff.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Api;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<BrawlBuffDbContext>();
                var brawlApiHttpService = services.GetRequiredService<BrawlApiHttpService>();
                var brawlBuffDbContext = scope.ServiceProvider.GetRequiredService<IBrawlBuffDbContext>();
                await brawlBuffDbContext.Database.MigrateAsync();
                await BrawlBuffDbContextSeed.SeedSampleDataAsync(context, brawlApiHttpService);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddAzureKeyVault();
            });
}