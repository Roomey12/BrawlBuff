using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsStats
{
    public class GetTeamsMapsStatsQuery : IRequest<GetTeamsMapsStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
