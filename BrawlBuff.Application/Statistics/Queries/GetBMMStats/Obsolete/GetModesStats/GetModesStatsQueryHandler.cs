using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetModesStats;

public class GetModesStatsQueryHandler : IRequestHandler<GetModesStatsQuery, GetModesStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;
    public GetModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetModesStatsQueryResult> Handle(GetModesStatsQuery request, CancellationToken cancellationToken)
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
            select new { Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetModesStatsQueryResult
        {
            ModesStats = await mapsBattleDetails
                .GroupBy(s => s.Mode)
                .Select(group => new ModeStatsDTO
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