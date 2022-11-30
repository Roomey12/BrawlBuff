using System.Diagnostics;
using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsModesStats;

public class GetBrawlersMapsModesStatsQueryHandler : IRequestHandler<GetBrawlersMapsModesStatsQuery, GetBrawlersMapsModesStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetBrawlersMapsModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetBrawlersMapsModesStatsQueryResult> Handle(GetBrawlersMapsModesStatsQuery request, CancellationToken cancellationToken)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var battleDetails = _brawlBuffDbContext.BattleDetails.AsQueryable();
        var isPersonal = !string.IsNullOrEmpty(request.PlayerTag);

        if (isPersonal)
        {
            battleDetails = battleDetails.Where(x => x.PlayerTag == request.PlayerTag);
        }

        var mapsModesBattleDetails =
            from battleDetail in battleDetails
            join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
            join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
            select new { Brawler = battleDetail.Brawler, Map = ev.Map, Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetBrawlersMapsModesStatsQueryResult
        {
            BrawlersMapsModesStats = await mapsModesBattleDetails
                .GroupBy(s => new { s.Brawler, s.Map, s.Mode })
                .Select(group => new BrawlerMapModeStatsDTO
                {
                    Brawler = group.Key.Brawler,
                    Map = group.Key.Map,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                //.OrderBy(x => x.BattlesCount)
                .OrderBy(x => x.Brawler)
                .ThenBy(x => x.Map)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };

        stopWatch.Stop();
        Console.WriteLine(stopWatch.ElapsedMilliseconds);
        return result;
    }
}