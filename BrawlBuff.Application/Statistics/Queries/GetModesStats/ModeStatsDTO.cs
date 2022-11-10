namespace BrawlBuff.Application.Statistics.Queries.GetModesStats;

public class ModeStatsDTO
{
    public string Map { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}