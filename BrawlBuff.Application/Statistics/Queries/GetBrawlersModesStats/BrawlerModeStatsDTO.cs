namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersModesStats;

public class BrawlerModeStatsDTO
{
    public string Brawler { get; set; }
    public string Mode { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}