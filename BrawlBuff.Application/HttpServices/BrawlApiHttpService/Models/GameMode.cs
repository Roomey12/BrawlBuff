namespace BrawlBuff.Application.HttpServices.BrawlApiHttpService.Models;

public class GameMode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Hash { get; set; }
    public int Version { get; set; }
    public string Color { get; set; }
    public string Link { get; set; }
    public string ImageUrl { get; set; }
}