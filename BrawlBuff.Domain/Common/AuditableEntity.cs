namespace BrawlBuff.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}