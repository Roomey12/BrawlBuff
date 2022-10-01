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
        private readonly IDateTime _dateTime;

        public RecordStatisticsService(ILogger<RecordStatisticsService> logger, 
            IBrawlBuffDbContext brawlBuffDbContext,
            IPlayerService playerService,
            IDateTime dateTime)
        {
            _logger = logger;
            _brawlBuffDbContext = brawlBuffDbContext;
            _playerService = playerService;
            _dateTime = dateTime;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{0} - Trying to record data.", _dateTime.Now);

                await RecordStatistics(stoppingToken);

                var minutes = 4;
                var delay = minutes * 60 * 1000;
                await Task.Delay(delay, stoppingToken);
            }
        }

        private async Task RecordStatistics(CancellationToken stoppingToken)
        {
            var delay = 35;
            var playersToUpdate = await _brawlBuffDbContext.Players
                .Where(x => x.StatsUpdatedOn < _dateTime.Now.AddMinutes(-delay))
                .ToListAsync(stoppingToken);
            
            foreach(var player in playersToUpdate)
            {
                _logger.LogInformation("{0} - Recording data for player {1}", _dateTime.Now, player.Tag);
                await _playerService.RecordPlayerBattleStatsAsync(player);
            }
        }
    }
}
