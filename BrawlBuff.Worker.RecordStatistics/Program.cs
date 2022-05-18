using BrawlBuff.Application;
using BrawlBuff.Infrastructure;
using BrawlBuff.Worker.RecordStatistics;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(hostContext.Configuration);
        services.AddHostedService<ConsumeRecordStatisticsHostedService>();
        services.AddScoped<IRecordStatisticsService, RecordStatisticsService>();
    })
    .Build();

await host.RunAsync();
