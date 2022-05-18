using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Worker.RecordStatistics
{
    internal class RecordStatisticsService : IRecordStatisticsService
    {
        private readonly ILogger _logger;
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;
        private readonly IPlayerService _playerService;

        public RecordStatisticsService(ILogger<RecordStatisticsService> logger, 
            IBrawlBuffDbContext brawlBuffDbContext,
            IPlayerService playerService)
        {
            _logger = logger;
           _brawlBuffDbContext = brawlBuffDbContext;
            _playerService = playerService;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{0} - Trying to record data.", DateTime.Now);

                await RecordStatistics();

                await Task.Delay(10000, stoppingToken);
            }
        }

        private async Task RecordStatistics()
        {
            var playersToUpdate = await _brawlBuffDbContext.Players.Where(x => x.StatsUpdatedOn < DateTime.Now.AddMinutes(-1)).ToListAsync();
            
            foreach(var player in playersToUpdate)
            {
                _logger.LogInformation("{0} - Recording data for player {1}", DateTime.Now, player.Tag);
                await _playerService.RecordPlayerBattleStatsAsync(player);
            }
        }
    }
}
