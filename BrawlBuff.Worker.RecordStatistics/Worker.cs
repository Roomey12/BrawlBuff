using BrawlBuff.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Worker.RecordStatistics
{
    //public class Worker : BackgroundService
    //{
    //    private readonly ILogger<Worker> _logger;
    //    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    //    public Worker(ILogger<Worker> logger, IBrawlBuffDbContext brawlBuffDbContext)
    //    {
    //        _logger = logger;
    //        _brawlBuffDbContext = brawlBuffDbContext;
    //    }


    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            var x = await _brawlBuffDbContext.Battles.FirstOrDefaultAsync();
    //            _logger.LogInformation(x.Id.ToString());
    //            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    //            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
    //        }
    //    }
    //}

    public class ConsumeScopedServiceHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}