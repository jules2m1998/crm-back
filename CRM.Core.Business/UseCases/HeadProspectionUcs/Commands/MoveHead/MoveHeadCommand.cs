using MediatR;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs.Commands.MoveHead;

public record MoveHeadCommand: IRequest<MoveHeadResponse>
{
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid AgentId { get; set; }
    public Guid ResponseId { get; set; }
    public string? Message { get; set; } = string.Empty;
}
