using CRM.Core.Business.Models.Product;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Extensions;

public static class EntityToModel
{
    public static ProductOutModel ToProductOutModel(this Product product)
    {
        var creator = product.Creator;
        return new ProductOutModel(product.Id, product.Name, product.Logo, product.Description, product.CreatedAt, product.UpdateAt, creator?.ToUserModel());
    }
}
