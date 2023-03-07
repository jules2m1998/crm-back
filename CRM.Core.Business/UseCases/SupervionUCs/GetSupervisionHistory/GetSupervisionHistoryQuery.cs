using CRM.Core.Business.Models.Supervision;
using MediatR;

namespace CRM.Core.Business.UseCases.SupervionUCs.GetSupervisionHistory;

public class GetSupervisionHistoryQuery: IRequest<ICollection<SupervisionOutModel>>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    public GetSupervisionHistoryQuery(Guid userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}
