using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetTeamsMapsModesStats
{
    public class TeamMapModeStatsDTO
    {
        public string TeammateTag { get; set; }
        public string Map { get; set; }
        public string Mode { get; set; }
        public int BattlesCount { get; set; }
        public int BattlesWonCount { get; set; }
        public int BattlesLostCount { get; set; }
        public double Winrate { get; set; }
    }
}
