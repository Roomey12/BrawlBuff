﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsModesStats
{
    public class GetTeamsModesStatsQuery : IRequest<GetTeamsModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
