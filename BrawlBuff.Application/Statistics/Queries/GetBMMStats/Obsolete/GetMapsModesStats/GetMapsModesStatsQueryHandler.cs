using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsModesStats;

public class GetMapsModesStatsQueryHandler : IRequestHandler<GetMapsModesStatsQuery, GetMapsModesStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetMapsModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetMapsModesStatsQueryResult> Handle(GetMapsModesStatsQuery request, CancellationToken cancellationToken)
    {
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
            select new { Map = ev.Map, Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetMapsModesStatsQueryResult
        {
            MapsModesStats = await mapsModesBattleDetails
                .GroupBy(s => new { s.Map, s.Mode })
                .Select(group => new MapModeStatsDTO
                {
                    Map = group.Key.Map,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderBy(x => x.Map)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}