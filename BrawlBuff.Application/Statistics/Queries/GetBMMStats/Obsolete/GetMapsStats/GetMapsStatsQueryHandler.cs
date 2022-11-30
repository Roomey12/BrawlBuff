using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsStats;

public class GetMapsStatsQueryHandler : IRequestHandler<GetMapsStatsQuery, GetMapsStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetMapsStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetMapsStatsQueryResult> Handle(GetMapsStatsQuery request, CancellationToken cancellationToken)
    {
        var battleDetails = _brawlBuffDbContext.BattleDetails.AsQueryable();
        var isPersonal = !string.IsNullOrEmpty(request.PlayerTag);

        if (isPersonal)
        {
            battleDetails = battleDetails.Where(x => x.PlayerTag == request.PlayerTag);
        }

        var mapsBattleDetails =
            from battleDetail in battleDetails
            join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
            join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
            select new { Map = ev.Map, BattleDetail = battleDetail };

        var result = new GetMapsStatsQueryResult
        {
            MapsStats = await mapsBattleDetails
                .GroupBy(s => s.Map)
                .Select(group => new MapStatsDTO
                {
                    Map = group.Key,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderBy(x => x.Map)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}