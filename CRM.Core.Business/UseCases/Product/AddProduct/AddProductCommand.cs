using CRM.Core.Business.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.AddProduct;

public class AddProductCommand: IRequest<ProductOutModel>
{
    public ProductInModel Product { get; set; }
    public string UserName { get; set; }

    public AddProductCommand(ProductInModel product, string userName)
    {
        Product = product;
        UserName = userName;
    }
}
