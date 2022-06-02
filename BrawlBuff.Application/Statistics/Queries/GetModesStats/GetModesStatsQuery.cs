﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetModesStats
{
    public class GetModesStatsQuery : IRequest<GetModesStatsQueryResult>
    {
        public string PlayerTag { get; set; }
    }
}
