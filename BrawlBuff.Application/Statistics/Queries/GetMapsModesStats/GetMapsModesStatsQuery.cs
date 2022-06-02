using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetMapsModesStats
{
    public class GetMapsModesStatsQuery : IRequest<GetMapsModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}