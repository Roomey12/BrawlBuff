using System;
using System.Linq;
using System.Threading.Tasks;
using BrawlBuff.Application.Common.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BrawlBuff.RecordStatistics;

public class RecordData
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;
    private readonly IPlayerService _playerService;
    private readonly ILogger _logger;
    private readonly IDateTime _dateTime;

    public RecordData(IBrawlBuffDbContext brawlBuffDbContext, IPlayerService playerService, ILogger<RecordData> logger, IDateTime dateTime)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
        _playerService = playerService;
        _logger = logger;
        _dateTime = dateTime;
        _logger.LogInformation("Record Statistics Function Constructor");
    }

    [FunctionName("RecordData")]//0 0 7-17/5 * * *
    public async Task Run([TimerTrigger("0 0 7-17/5 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Record Data function executed at: {_dateTime.Now}");

        var compareTime = DateTime.Now.AddMinutes(-1);

        var playersToUpdate = await _brawlBuffDbContext.Players
            .Where(x => x.StatsUpdatedOn < compareTime)
            .ToListAsync();

        foreach (var player in playersToUpdate)
        {
            _logger.LogInformation("{0} - Staring recording data for player {1}", DateTime.Now, player.Tag);
            await _playerService.RecordPlayerBattleStatsAsync(player);
            _logger.LogInformation("{0} - Finished recording data for player {1}", DateTime.Now, player.Tag);
        }

        _logger.LogInformation($"Record Data function finished at: {_dateTime.Now}");

    }
}