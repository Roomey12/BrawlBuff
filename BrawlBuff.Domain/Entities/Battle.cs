using BrawlBuff.Domain.Common;

namespace BrawlBuff.Domain.Entities;

public class Battle : AuditableEntity
{
    public int Id { get; set; }
    public Event? Event { get; set; }
    public int? EventId { get; set; }
    public string? StarPlayerTag { get; set; }
    public string? Type { get; set; }
    public DateTime BattleTime { get; set; }
    public int? Duration { get; set; }
}