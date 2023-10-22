namespace CRM.Core.Domain.Entities;

public class HeadProspection
{
    public Guid CommitId { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid AgentId { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActivated { get; set; } = true;
    virtual public User? Creator { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual User Agent { get; set; } = null!;
    public virtual Commit Commit { get; set; } = null!;
}
