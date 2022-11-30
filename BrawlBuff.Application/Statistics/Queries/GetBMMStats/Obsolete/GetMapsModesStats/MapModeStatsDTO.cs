namespace BrawlBuff.Application.Statistics.Queries.GetMapsModesStats;

public class MapModeStatsDTO
{
    public string Map { get; set; }
    public string Mode { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}