using CRM.Core.Business.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.GetOneProduct;

public class GetOneProductQuery: IRequest<ProductOutModel>
{
    public string UserName { get; set; }
    public Guid Id { get; set; }

    public GetOneProductQuery(string userName, Guid id)
    {
        UserName = userName;
        Id = id;
    }
}
