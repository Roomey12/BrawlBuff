using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetPersonalStats;

public class GetPlayerStatsQueryHandler : IRequestHandler<GetPlayerStatsQuery, GetPlayerStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;
    private readonly BrawlStarsApiHttpService _brawlStarsApiHttpService;

    public GetPlayerStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext, BrawlStarsApiHttpService brawlStarsApiHttpService)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
        _brawlStarsApiHttpService = brawlStarsApiHttpService;
    }

    public async Task<GetPlayerStatsQueryResult> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
    {
        var battleDetails = _brawlBuffDbContext.BattleDetails
            .Where(x => x.PlayerTag == request.PlayerTag);
        var battlesCount = await battleDetails
            .CountAsync(cancellationToken);
        var battlesWonCount = await battleDetails
            .CountAsync(x => x.Result == BattleResult.Victory.GetString(), cancellationToken);
        var starPlayerCount = await battleDetails
            .Where(x => x.Battle.StarPlayerTag == request.PlayerTag)
            .CountAsync(cancellationToken);

        var apiPlayer = await _brawlStarsApiHttpService.GetPlayerByTagAsync(request.PlayerTag, true);

        var result = new GetPlayerStatsQueryResult()
        {
            BattlesCount = battlesCount,
            BattlesWonCount = battlesWonCount,
            BattlesLostCount = battlesCount - battlesWonCount,
            Winrate = (double)battlesWonCount / battlesCount,
            StarPlayerCount = starPlayerCount,
            CurrentTrophiesCount= apiPlayer.Trophies,
            MaxTrophiesCount = apiPlayer.HighestTrophies,
            Level = apiPlayer.ExpLevel
        };
        return result;
    }
}