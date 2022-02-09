using BrawlBuff.Infrastructure;
using BrawlBuff.Worker.RecordStatistics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure(hostContext.Configuration);
        services.AddHostedService<ConsumeScopedServiceHostedService>();
        services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
        //services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();


//namespace BrawlBuff.Worker.RecordStatistics
//{
//    public class Program
//    {
//        public static async Task Main(string[] args)
//        {
//            await CreateHostBuilder(args).Build().RunAsync();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                }).ConfigureServices(services =>
//                    services.AddHostedService<Worker>());
//    }
//}