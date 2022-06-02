using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsStats
{
    public class TeamStatsDTO
    {
        public string TeammateTag { get; set; }
        public int BattlesCount { get; set; }
        public int BattlesWonCount { get; set; }
        public int BattlesLostCount { get; set; }
        public double Winrate { get; set; }
    }
}
