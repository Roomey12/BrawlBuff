using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Brawlers.Queries.GetBrawlersStats
{
    public class GetBrawlersStatsQuery : IRequest<GetBrawlersStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
