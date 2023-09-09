namespace CRM.Core.Domain.Entities;

public class StageResponse: BaseEntity
{
    public string Response { get; set; } = string.Empty;
    public Guid? NextStageId { get; set; }
    public Guid StageId { get; set; }

    public virtual ProductStage Stage { get; set; } = null!;
    public virtual ProductStage? NextStage { get; set; } = null;
}
