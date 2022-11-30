using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetTMMStats;

public class GetTMMStatsQueryHandler : IRequestHandler<GetTMMStatsQuery, GetTMMStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetTMMStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetTMMStatsQueryResult> Handle(GetTMMStatsQuery request, CancellationToken cancellationToken)
    {
        var teamIds = _brawlBuffDbContext.BattleDetails
            .Where(x => x.PlayerTag == request.PlayerTag && x.TeamId != null)
            .Select(x => x.TeamId);

        var battleDetails = _brawlBuffDbContext.BattleDetails
            .Where(x => teamIds.Any(t => t.Value == x.TeamId) && x.PlayerTag != request.PlayerTag);

        var teamsMapsModesBattleDetails =
            from battleDetail in battleDetails
            join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
            join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
            select new { TeammateTag = battleDetail.PlayerTag, Map = ev.Map, Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetTMMStatsQueryResult
        {
            TeamsMapsModesStats = await teamsMapsModesBattleDetails
                .GroupBy(s => new
                {
                    TeammateTag = request.IsTeamRequest ? s.TeammateTag : null,
                    Map = request.IsMapRequest ? s.Map : null,
                    Mode = request.IsModeRequest ? s.Mode : null
                })
                .Select(group => new TMMStatsDTO
                {
                    TeammateTag = group.Key.TeammateTag,
                    Map = group.Key.Map,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count(x => x.BattleDetail.Result == BattleResult.Defeat.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .Where(x => x.BattlesCount > 1)
                .OrderByDescending(x => x.BattlesCount)
                .ThenBy(x => x.TeammateTag)
                .ThenBy(x => x.Map)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}
