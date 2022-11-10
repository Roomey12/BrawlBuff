﻿namespace BrawlBuff.Application.Statistics.Queries.GetBrawlersMapsStats;

public class BrawlerMapStatsDTO
{
    public string Brawler { get; set; }
    public string Map { get; set; }
    public int BattlesCount { get; set; }
    public int BattlesWonCount { get; set; }
    public int BattlesLostCount { get; set; }
    public double Winrate { get; set; }
}