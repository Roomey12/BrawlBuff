using BrawlBuff.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Worker.RecordStatistics
{
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IBrawlBuffDbContext brawlBuffDbContext)
        {
            _logger = logger;
           _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                var x = await _brawlBuffDbContext.Battles.FirstOrDefaultAsync(x => x.Id == 16);
                _logger.LogInformation(x.Id.ToString());
                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
