using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetPersonalStats
{
    public class GetPlayerStatsQueryResult
    {
        public int BattlesCount { get; set; }
        public int BattlesWonCount { get; set; }
        public int BattlesLostCount { get; set; }
        public double Winrate { get; set; }
        public int StarPlayerCount { get; set; }
        public int CurrentTrophiesCount { get; set; }
        public int MaxTrophiesCount { get; set; }
        public int Level { get; set; }
    }
}
