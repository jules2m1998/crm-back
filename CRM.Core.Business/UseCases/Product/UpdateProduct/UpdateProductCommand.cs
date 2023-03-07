using CRM.Core.Business.Models.Product;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.UpdateProduct;

public class UpdateProductCommand: IRequest<ProductOutModel>
{
    public JsonPatchDocument<Domain.Entities.Product> Product { get; set; }
    public Guid Id { get; set; }
    public string UserName { get; set; }

    public UpdateProductCommand(JsonPatchDocument<Domain.Entities.Product> product, Guid id, string userName)
    {
        Product = product;
        Id = id;
        UserName = userName;
    }
}
