using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsModesStats
{
    public class GetBrawlersMapsModesStatsQuery : IRequest<GetBrawlersMapsModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
