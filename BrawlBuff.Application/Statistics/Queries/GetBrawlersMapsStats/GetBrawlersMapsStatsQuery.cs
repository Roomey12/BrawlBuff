using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsStats
{
    public class GetBrawlersMapsStatsQuery : IRequest<GetBrawlersMapsStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
