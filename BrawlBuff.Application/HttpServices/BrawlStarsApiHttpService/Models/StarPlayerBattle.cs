namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

public class StarPlayerBattle
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public BrawlerBattle Brawler { get; set; }
}