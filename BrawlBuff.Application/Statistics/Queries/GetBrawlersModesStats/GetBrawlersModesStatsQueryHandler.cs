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

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersModesStats
{
    public class GetBrawlersModesStatsQueryHandler : IRequestHandler<GetBrawlersModesStatsQuery, GetBrawlersModesStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetBrawlersModesStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<GetBrawlersModesStatsQueryResult> Handle(GetBrawlersModesStatsQuery request, CancellationToken cancellationToken)
        {
            var battleDetails = _brawlBuffDbContext.BattleDetails.AsQueryable();
            var isPersonal = !string.IsNullOrEmpty(request.PlayerTag);

            if (isPersonal)
            {
                battleDetails = battleDetails.Where(x => x.PlayerTag == request.PlayerTag);
            }

            var brawlersModesBattleDetails =
                from battleDetail in battleDetails
                join battle in _brawlBuffDbContext.Battles on battleDetail.BattleId equals battle.Id
                join ev in _brawlBuffDbContext.Events on battle.EventId equals ev.Id
                select new { Brawler = battleDetail.Brawler, Mode = ev.Mode, BattleDetail = battleDetail };

            var result = new GetBrawlersModesStatsQueryResult
            {
                BrawlersModesStats = await brawlersModesBattleDetails.GroupBy(s => new { s.Brawler, s.Mode })
                .Select(group => new BrawlerModeStatsDTO
                {
                    Brawler = group.Key.Brawler,
                    Mode = group.Key.Mode,
                    BattlesCount = group.Count(),
                    BattlesWonCount = group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    BattlesLostCount = group.Count() - group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()),
                    Winrate = (double)group.Count(x => x.BattleDetail.Result == BattleResult.Victory.GetString()) / group.Count()
                })
                .OrderBy(x => x.BattlesCount)
                .ThenBy(x => x.Brawler)
                .ThenBy(x => x.Mode)
                .ToListAsync(cancellationToken)
            };

            return result;
        }
    }
}
