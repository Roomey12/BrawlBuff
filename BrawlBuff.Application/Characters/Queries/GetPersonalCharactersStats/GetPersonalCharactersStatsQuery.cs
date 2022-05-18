using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Characters.Queries.GetPersonalCharactersStats
{
    public class GetPersonalCharactersStatsQuery : IRequest<GetPersonalCharactersStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
