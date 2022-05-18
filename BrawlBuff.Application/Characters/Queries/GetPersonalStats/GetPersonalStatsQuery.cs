using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Characters.Queries.GetPersonalStats
{
    public class GetPersonalStatsQuery : IRequest<GetPersonalStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
