using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsModesStats;

public class GetTeamsMapsModesStatsQueryHandler : IRequestHandler<GetTeamsMapsModesStatsQuery, GetTeamsMapsModesStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetTeamsMapsModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetTeamsMapsModesStatsQueryResult> Handle(GetTeamsMapsModesStatsQuery request, CancellationToken cancellationToken)
    {
        var teamIds = _brawlBuffDbContext.BattleDetails
            .Where(x => x.PlayerTag == request.PlayerTag && x.TeamId != null)
            .Select(x => x.TeamId);

        var battleDetails = _brawlBuffDbContext.BattleDetails
            .Where(x => teamIds.Contains(x.TeamId) && x.PlayerTag != request.PlayerTag);

        var teamsMapsModesBattleDetails =
            from battleDetail in battleDetails
            join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
            join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
            select new { TeammateTag = battleDetail.PlayerTag, Map = ev.Map, Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetTeamsMapsModesStatsQueryResult
        {
            TeamsMapsModesStats = await teamsMapsModesBattleDetails
                .GroupBy(s => new { s.TeammateTag, s.Map, s.Mode })
                .Select(group => new TeamMapModeStatsDTO
                {
                    TeammateTag = group.Key.TeammateTag,
                    Map = group.Key.Map,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderByDescending(x => x.BattlesCount)
                .ThenBy(x => x.TeammateTag)
                .ThenBy(x => x.Map)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}