using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsStats
{
    public class GetMapsStatsQuery : IRequest<GetMapsStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
