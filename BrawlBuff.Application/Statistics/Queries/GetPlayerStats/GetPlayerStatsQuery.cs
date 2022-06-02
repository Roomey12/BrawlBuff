using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetPersonalStats
{
    public class GetPlayerStatsQuery : IRequest<GetPlayerStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
