namespace BrawlBuff.Application.HttpServices.BrawlApiHttpService.Models;

public class Map
{
    public int Id { get; set; }
    public bool New { get; set; }
    public bool Disabled { get; set; }
    public string Name { get; set; }
    public string Hash { get; set; }
    public int Version { get; set; }
    public string Link { get; set; }
    public string ImageUrl { get; set; }
    public string Credit { get; set; }
    public Environment Environment { get; set; }
    public GameMode GameMode { get; set; }
    public int? LastActive { get; set; }
    public int? DataUpdated { get; set; }
}