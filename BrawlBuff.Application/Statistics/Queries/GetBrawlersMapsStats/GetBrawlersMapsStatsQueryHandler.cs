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

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsStats
{
    public class GetBrawlersMapsStatsQueryHandler : IRequestHandler<GetBrawlersMapsStatsQuery, GetBrawlersMapsStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetBrawlersMapsStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<GetBrawlersMapsStatsQueryResult> Handle(GetBrawlersMapsStatsQuery request, CancellationToken cancellationToken)
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
                select new { Map = ev.Map, Brawler = battleDetail.Brawler, BattleDetail = battleDetail };

            var result = new GetBrawlersMapsStatsQueryResult
            {
                BrawlersMapsStats = await mapsModesBattleDetails
                .GroupBy(s => new { s.Brawler, s.Map })
                .Select(group => new BrawlerMapStatsDTO
                {
                    Brawler = group.Key.Brawler,
                    Map = group.Key.Map,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderBy(x => x.Brawler)
                .ThenBy(x => x.Map)
                .ToListAsync(cancellationToken)
            };

            return result;
        }
    }
}
