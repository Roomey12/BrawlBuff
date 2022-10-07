using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BrawlBuff.Application.Common.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BrawlBuff.RecordStatistics
{
    public class RecordData
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;
        private readonly IPlayerService _playerService;
        private readonly ILogger log;

        public RecordData(IBrawlBuffDbContext brawlBuffDbContext, IPlayerService playerService, ILogger<RecordData> log)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
            _playerService = playerService;
            this.log = log;
            log.LogInformation("Ctor");
        }

        [FunctionName("RecordData")]//0 0 10-22/2 * * *
        public async Task Run([TimerTrigger("0 0 7-17/5 * * *")] TimerInfo myTimer)
        {
            log.LogInformation($"Record Data function executed at: {DateTime.Now}");

            var compareTime = DateTime.Now.AddMinutes(-1);

            var playersToUpdate = await _brawlBuffDbContext.Players
                .Where(x => x.StatsUpdatedOn < compareTime)
                .ToListAsync();

            foreach (var player in playersToUpdate)
            {
                log.LogInformation("{0} - Staring recording data for player {1}", DateTime.Now, player.Tag);
                await _playerService.RecordPlayerBattleStatsAsync(player);
                log.LogInformation("{0} - Finished recording data for player {1}", DateTime.Now, player.Tag);
            }

            log.LogInformation($"Record Data function finished at: {DateTime.Now}");

        }
    }
}
