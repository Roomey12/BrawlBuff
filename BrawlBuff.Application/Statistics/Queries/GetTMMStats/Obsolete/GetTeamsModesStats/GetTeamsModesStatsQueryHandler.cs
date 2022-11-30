using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsModesStats;

public class GetTeamsModesStatsQueryHandler : IRequestHandler<GetTeamsModesStatsQuery, GetTeamsModesStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetTeamsModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetTeamsModesStatsQueryResult> Handle(GetTeamsModesStatsQuery request, CancellationToken cancellationToken)
    {
        var teamIds = _brawlBuffDbContext.BattleDetails
            .Where(x => x.PlayerTag == request.PlayerTag && x.TeamId != null)
            .Select(x => x.TeamId);

        var battleDetails = _brawlBuffDbContext.BattleDetails
            .Where(x => teamIds.Contains(x.TeamId) && x.PlayerTag != request.PlayerTag);

        var teamsModesBattleDetails =
            from battleDetail in battleDetails
            join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
            join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
            select new { TeammateTag = battleDetail.PlayerTag, Mode = ev.Mode, BattleDetail = battleDetail };

        var result = new GetTeamsModesStatsQueryResult
        {
            TeamsModesStats = await teamsModesBattleDetails
                .GroupBy(s => new { s.TeammateTag, s.Mode })
                .Select(group => new TeamModeStatsDTO
                {
                    TeammateTag = group.Key.TeammateTag,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderByDescending(x => x.BattlesCount)
                .ThenBy(x => x.TeammateTag)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}