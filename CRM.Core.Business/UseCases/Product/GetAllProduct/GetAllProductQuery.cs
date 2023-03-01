using CRM.Core.Business.Models.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.GetAllProduct;

public record GetAllProductQuery(string UserName) : IRequest<ICollection<ProductOutModel>>;
