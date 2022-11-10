namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

public class Battle
{
    public string Mode { get; set; }
    public string Type { get; set; }
    public int? TrophyChange { get; set; }
    public List<PlayerBattle> Players { get; set; }
    public string Result { get; set; }
    public int? Duration { get; set; }
    public int? Rank { get; set; }
    public StarPlayerBattle StarPlayer { get; set; }
    public List<List<PlayerBattle>> Teams { get; set; }
}