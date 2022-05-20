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

namespace BrawlBuff.Application.Players.Queries.GetPersonalStats
{
    public class GetPlayerStatsQueryHandler : IRequestHandler<GetPlayerStatsQuery, GetPlayerStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetPlayerStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }
        public async Task<GetPlayerStatsQueryResult> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
        {
            var battleDetails = _brawlBuffDbContext.BattleDetails
                .Where(x => x.PlayerTag == request.PlayerTag);

            var battlesCount = await battleDetails
                .CountAsync(cancellationToken);
            var battlesWonCount = await battleDetails
                .CountAsync(x => x.Result == BattleResult.Victory.GetString(), cancellationToken);
            var winrate = (double) battlesWonCount / battlesCount;

            var result = new GetPlayerStatsQueryResult()
            {
                BattlesCount = battlesCount,
                BattlesWonCount = battlesWonCount,
                Winrate = winrate,
            };
            return result;
        }
    }
}
