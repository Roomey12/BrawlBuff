using BrawlBuff.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Worker.RecordStatistics
{
    public class ConsumeRecordStatisticsHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeRecordStatisticsHostedService> _logger;
        public IServiceProvider Services { get; }

        public ConsumeRecordStatisticsHostedService(IServiceProvider services,
            ILogger<ConsumeRecordStatisticsHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{0} - Starting RecordStatistics Worker.", DateTime.Now);

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{0} - RecordStatistics Worker is running.", DateTime.Now);

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IRecordStatisticsService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{0} - RecordStatistics Worker is stopped.", DateTime.Now);


            await base.StopAsync(stoppingToken);
        }
    }
}