using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersModesStats
{
    public class GetBrawlersModesStatsQuery : IRequest<GetBrawlersModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
