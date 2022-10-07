using BrawlBuff.Application;
using BrawlBuff.Infrastructure;
using BrawlBuff.Infrastructure.Extensions;
using BrawlBuff.Worker.RecordStatistics;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var buildConfig = config.Build();
        config.AddAzureKeyVault();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(hostContext.Configuration);
        services.AddHostedService<ConsumeRecordStatisticsHostedService>();
        services.AddScoped<IRecordStatisticsService, RecordStatisticsService>();
    })
    .Build();

await host.RunAsync();
