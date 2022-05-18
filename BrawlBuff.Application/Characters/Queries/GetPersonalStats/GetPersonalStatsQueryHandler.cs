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

namespace BrawlBuff.Application.Characters.Queries.GetPersonalStats
{
    public class GetPersonalStatsQueryHandler : IRequestHandler<GetPersonalStatsQuery, GetPersonalStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetPersonalStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }
        public async Task<GetPersonalStatsQueryResult> Handle(GetPersonalStatsQuery request, CancellationToken cancellationToken)
        {
            var battleDetails = _brawlBuffDbContext.BattleDetails.Where(x => x.PlayerTag == request.PlayerTag);

            var battlesCount = await battleDetails
                .CountAsync(cancellationToken);
            var battlesWonCount = await battleDetails
                .CountAsync(x => x.Result == BattleResult.Victory.GetString(), cancellationToken);
            var winrate = (double) battlesWonCount / battlesCount;

            var result = new GetPersonalStatsQueryResult()
            {
                Winrate = winrate
            };
            return result;
        }
    }
}
