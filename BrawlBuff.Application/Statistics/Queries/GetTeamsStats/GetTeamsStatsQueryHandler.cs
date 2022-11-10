using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsStats;

public class GetTeamsStatsQueryHandler : IRequestHandler<GetTeamsStatsQuery, GetTeamsStatsQueryResult>
{
    private readonly IBrawlBuffDbContext _brawlBuffDbContext;

    public GetTeamsStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
    {
        _brawlBuffDbContext = brawlBuffDbContext;
    }

    public async Task<GetTeamsStatsQueryResult> Handle(GetTeamsStatsQuery request, CancellationToken cancellationToken)
    {
        var teamIds = _brawlBuffDbContext.BattleDetails
            .Where(x => x.PlayerTag == request.PlayerTag && x.TeamId != null)
            .Select(x => x.TeamId);

        var result = new GetTeamsStatsQueryResult
        {
            TeamsStats = await _brawlBuffDbContext.BattleDetails
                .Where(x => teamIds.Contains(x.TeamId) && x.PlayerTag != request.PlayerTag)
                .GroupBy(x => x.PlayerTag)
                .Select(group => new TeamStatsDTO
                {
                    TeammateTag = group.Key,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderByDescending(x => x.BattlesCount)
                .ToListAsync(cancellationToken)
        };

        return result;
    }
}