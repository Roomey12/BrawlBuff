namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;

public class BattleLog
{
    public string BattleTime { get; set; }
    public EventBattle Event { get; set; }
    public Battle Battle { get; set; }
}