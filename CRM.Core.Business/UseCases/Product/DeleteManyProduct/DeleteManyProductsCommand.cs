using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.DeleteManyProduct;

public class DeleteManyProductsCommand: IRequest<bool>
{
    public ICollection<Guid> Ids { get; set; }
    public string UserName { get; set; }

    public DeleteManyProductsCommand(ICollection<Guid> ids, string userName)
    {
        Ids = ids;
        UserName = userName;
    }
}
