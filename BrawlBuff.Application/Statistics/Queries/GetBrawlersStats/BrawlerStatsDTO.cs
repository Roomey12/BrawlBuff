using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersStats
{
    public class BrawlerStatsDTO
    {
        public string Brawler { get; set; }
        public int BattlesCount { get; set; }
        public int BattlesWonCount { get; set; }
        public int BattlesLostCount { get; set; }
        public double Winrate { get; set; }
        public int Rank { get; set; }
        public int Power { get; set; }
        public int CurrentTrophiesCount { get; set; }
        public int MaxTrophiesCount { get; set; }
    }
}
