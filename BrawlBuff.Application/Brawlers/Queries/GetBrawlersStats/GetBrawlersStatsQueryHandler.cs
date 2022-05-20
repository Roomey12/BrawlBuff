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

namespace BrawlBuff.Application.Brawlers.Queries.GetBrawlersStats
{
    public class GetBrawlersStatsQueryHandler : IRequestHandler<GetBrawlersStatsQuery, GetBrawlersStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetBrawlersStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<GetBrawlersStatsQueryResult> Handle(GetBrawlersStatsQuery request, CancellationToken cancellationToken)
        {
            var battleDetails = _brawlBuffDbContext.BattleDetails.AsQueryable();

            if(!string.IsNullOrEmpty(request.PlayerTag))
            {
                battleDetails = battleDetails.Where(x => x.PlayerTag == request.PlayerTag);
            }

            var result = new GetBrawlersStatsQueryResult
            {
                BrawlersStatistics = await battleDetails
                    .GroupBy(x => x.Brawler)
                    .Select(group => new BrawlerStatsDTO
                    {
                        Brawler = group.Key,
                        BattlesCount = group.Count(),
                        BattlesWonCount = group.Count(x => x.Result == BattleResult.Victory.GetString()),
                        Winrate = (double) group.Count(x => x.Result == BattleResult.Victory.GetString()) / group.Count()
                    })
                    .OrderByDescending(x => x.Winrate)
                    .ToListAsync(cancellationToken)
            };
            return result;
        }
    }
}
