using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService;
using BrawlBuff.Infrastructure.Persistence;

namespace BrawlBuff.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<BrawlBuffDbContext>();
                    var brawlApiHttpService = services.GetRequiredService<BrawlApiHttpService>();

                    await BrawlBuffDbContextSeed.SeedSampleDataAsync(context, brawlApiHttpService);
                }
                catch (Exception ex)
                {
                    //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    //logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
    }
}






























//using BrawlBuff.Application;
//using BrawlBuff.Infrastructure;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddApplication();
//builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.MapControllers();

//app.Run();
