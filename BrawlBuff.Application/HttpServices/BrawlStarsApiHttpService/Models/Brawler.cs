namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

public class Brawler
{
    public List<StarPower> StarPowers { get; set; }

    public List<Gadget> Gadgets { get; set; }

    public int Id { get; set; }

    public int Rank { get; set; }

    public int Trophies { get; set; }

    public int HighestTrophies { get; set; }

    public int Power { get; set; }

    public string Name { get; set; }
}