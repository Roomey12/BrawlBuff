using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsStats
{
    public class GetTeamsStatsQuery : IRequest<GetTeamsStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
