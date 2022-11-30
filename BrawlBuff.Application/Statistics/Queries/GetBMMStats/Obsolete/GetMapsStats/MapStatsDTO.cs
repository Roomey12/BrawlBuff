namespace BrawlBuff.Application.Statistics.Queries.GetMapsStats;

public class MapStatsDTO
{
    public string Map { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}