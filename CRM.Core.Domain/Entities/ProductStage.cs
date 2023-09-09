namespace CRM.Core.Domain.Entities;

public class ProductStage: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int StageLevel { get; set; } = 0;
    public string Question { get; set; } = string.Empty;
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
    public virtual ICollection<StageResponse> Responses { get; set; } = new List<StageResponse>();
}
