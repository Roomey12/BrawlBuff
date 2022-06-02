using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsModesStats
{
    public class GetTeamsMapsModesStatsQuery : IRequest<GetTeamsMapsModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
