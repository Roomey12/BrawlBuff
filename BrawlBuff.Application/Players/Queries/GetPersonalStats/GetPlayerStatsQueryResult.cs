using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Players.Queries.GetPersonalStats
{
    public class GetPlayerStatsQueryResult
    {
        public int BattlesCount { get; set; }
        public int BattlesWonCount { get; set; }
        public double Winrate { get; set; }
    }
}
