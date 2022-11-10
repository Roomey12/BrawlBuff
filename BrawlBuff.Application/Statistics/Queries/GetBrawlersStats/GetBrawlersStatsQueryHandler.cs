using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersStats;

public class GetBrawlersStatsQueryHandler : IRequestHandler<GetBrawlersStatsQuery, GetBrawlersStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;
    private readonly BrawlStarsApiHttpService _brawlStarsApiHttpService;

    public GetBrawlersStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext, BrawlStarsApiHttpService brawlStarsApiHttpService)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
        _brawlStarsApiHttpService = brawlStarsApiHttpService;
    }

    public async Task<GetBrawlersStatsQueryResult> Handle(GetBrawlersStatsQuery request, CancellationToken cancellationToken)
    {
        var battleDetails = _brawlBuffDbContext.BattleDetails.AsQueryable();
        var isPersonal = !string.IsNullOrEmpty(request.PlayerTag);

        List<Brawler> apiBrawlers = null;
        if (isPersonal)
        {
            battleDetails = battleDetails.Where(x => x.PlayerTag == request.PlayerTag);
            apiBrawlers = (await _brawlStarsApiHttpService.GetPlayerByTagAsync(request.PlayerTag, true)).Brawlers;
        }

        var brawlers = await battleDetails
            .GroupBy(x => x.Brawler)
            .Select(group => new
            {
                Brawler = group.Key,
                BattlesCount = group.Count(),
                BattlesWonCount = group.Count(x => x.Result == BattleResult.Victory.GetString())
            })
            .ToListAsync(cancellationToken);

        var result = new GetBrawlersStatsQueryResult
        {
            BrawlersStatistics = brawlers
                .Select(x => new BrawlerStatsDTO
                {
                    Brawler = x.Brawler,
                    BattlesCount = x.BattlesCount,
                    BattlesWonCount = x.BattlesWonCount,
                    BattlesLostCount = x.BattlesCount - x.BattlesWonCount,
                    Winrate = (double)x.BattlesWonCount / x.BattlesCount,
                    Rank = isPersonal ? apiBrawlers.First(o => o.Name == x.Brawler).Rank : 0,
                    Power = isPersonal ? apiBrawlers.First(o => o.Name == x.Brawler).Power : 0,
                    CurrentTrophiesCount = isPersonal ? apiBrawlers.First(o => o.Name == x.Brawler).Trophies : 0,
                    MaxTrophiesCount = isPersonal ? apiBrawlers.First(o => o.Name == x.Brawler).HighestTrophies : 0
                })
                .OrderByDescending(x => x.BattlesCount)
                .ThenBy(x => x.Brawler)
                .ToList()
        };
        return result;
    }
}