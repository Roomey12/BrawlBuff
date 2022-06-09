using BrawlBuff.Application.Common.Interfaces;
using BrawlBuff.Domain.Enums;
using BrawlBuff.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsStats
{
    public class GetTeamsMapsStatsQueryHandler : IRequestHandler<GetTeamsMapsStatsQuery, GetTeamsMapsStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetTeamsMapsStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<GetTeamsMapsStatsQueryResult> Handle(GetTeamsMapsStatsQuery request, CancellationToken cancellationToken)
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
                select new { TeammateTag = battleDetail.PlayerTag, Map = ev.Map, BattleDetail = battleDetail };

            var result = new GetTeamsMapsStatsQueryResult
            {
                TeamsMapsStats = await teamsModesBattleDetails
                .GroupBy(s => new { s.TeammateTag, s.Map })
                .Select(group => new TeamMapStatsDTO
                {
                    TeammateTag = group.Key.TeammateTag,
                    Map = group.Key.Map,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderByDescending(x => x.BattlesCount)
                .ThenBy(x => x.TeammateTag)
                .ThenBy(x => x.Map)
                .ToListAsync(cancellationToken)
            };

            return result;
        }
    }
}
