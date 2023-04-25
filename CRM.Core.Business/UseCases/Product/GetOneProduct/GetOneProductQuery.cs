using CRM.Core.Business.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.GetOneProduct;

public class GetOneProductQuery: IRequest<ProductOutModel?>
{
    public Guid Id { get; set; }

    public GetOneProductQuery(Guid id)
    {
        Id = id;
    }
}
