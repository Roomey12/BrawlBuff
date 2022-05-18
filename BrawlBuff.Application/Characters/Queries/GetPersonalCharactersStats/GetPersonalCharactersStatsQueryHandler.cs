using BrawlBuff.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Characters.Queries.GetPersonalCharactersStats
{
    public class GetPersonalCharactersStatsQueryHandler : IRequestHandler<GetPersonalCharactersStatsQuery, GetPersonalCharactersStatsQueryResult>
    {
        private readonly IBrawlBuffDbContext _brawlBuffDbContext;

        public GetPersonalCharactersStatsQueryHandler(IBrawlBuffDbContext brawlBuffDbContext)
        {
            _brawlBuffDbContext = brawlBuffDbContext;
        }

        public async Task<GetPersonalCharactersStatsQueryResult> Handle(GetPersonalCharactersStatsQuery request, CancellationToken cancellationToken)
        {
            var charactersStats = _brawlBuffDbContext.BattleDetails.Where(x => x.PlayerTag == request.PlayerTag);

            throw new NotImplementedException();
        }
    }
}
