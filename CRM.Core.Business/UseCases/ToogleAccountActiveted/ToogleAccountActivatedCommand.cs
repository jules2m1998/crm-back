using CRM.Core.Business.Models;
using MediatR;

namespace CRM.Core.Business.UseCases.ToogleAccountActiveted;

public class ToogleAccountActivatedCommand: IRequest<ICollection<UserModel>>
{
    public ICollection<Guid> Ids { get; set; } = new List<Guid>();
    public string UserName { get; set; } = string.Empty;
}
