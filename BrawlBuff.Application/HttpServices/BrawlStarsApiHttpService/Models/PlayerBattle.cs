namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

public class PlayerBattle
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public BrawlerBattle Brawler { get; set; }
    public List<BrawlerBattle> Brawlers { get; set; }
}