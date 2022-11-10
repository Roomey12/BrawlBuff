using BrawlBuff.Domain.Common;

namespace BrawlBuff.Domain.Entities;

public class BattleDetail : AuditableEntity
{
    public int Id { get; set; }
    public string PlayerTag { get; set; }
    public int? TrophyChange { get; set; }
    public int? Place { get; set; }
    public string? Brawler { get; set; }
    public string? Result { get; set; }

    public int BattleId { get; set;}
    public Battle Battle { get; set; }

    public int? PlayerId { get; set; }
    public Player Player { get; set; }

    public int? TeamId { get; set; }
    public Team Team { get; set; }
}