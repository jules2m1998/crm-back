namespace CRM.Core.Domain.Entities;

public class Commit: BaseEntity
{
    public Guid? ParentId { get; set; }
    public Guid? ResponseId { get; set; }

    public string Message { get; set; } = string.Empty;

    public virtual Commit? Parent { get; set; }
    public virtual StageResponse? Response { get; set; }
}
