﻿namespace BrawlBuff.Application.Statistics.Queries.GetTMMStats;

public class TMMStatsDTO
{
    public string TeammateTag { get; set; }
    public string Map { get; set; }
    public string Mode { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}