using System.Diagnostics;
using System.Linq.Expressions;
using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetBMMStats;

public class GetBMMStatsQueryHandler : IRequestHandler<GetBMMStatsQuery, GetBMMStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetBMMStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetBMMStatsQueryResult> Handle(GetBMMStatsQuery request, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
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

        var result = new GetBMMStatsQueryResult()
        {
            BrawlersMapsModesStats = await mapsModesBattleDetails
                .GroupBy(s => new
                {
                    Brawler = request.IsBrawlerRequest ? s.Brawler : null,
                    Map = request.IsMapRequest ? s.Map : null,
                    Mode = request.IsModeRequest ? s.Mode : null
                })
                .Select(group => new BMMStatsDTO()
                {
                    Brawler = group.Key.Brawler,
                    Map = group.Key.Map,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count(x => x.BattleDetail.Result == BattleResult.Defeat.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderBy(x => x.Brawler)
                .ThenBy(x => x.Map)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        return result;
    }
}