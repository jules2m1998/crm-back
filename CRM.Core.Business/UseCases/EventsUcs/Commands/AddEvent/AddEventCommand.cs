using MediatR;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;

public class AddEventCommand : IRequest<AddEventCommandResponse>
{
    #region prospection ids
    public Guid? ProductId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? AgentId { get; set; }
    #endregion

    public Guid OwnerId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Topic { get; set; } = null!;

    public ICollection<Guid> ContactIds { get; set; } = null!;
}
