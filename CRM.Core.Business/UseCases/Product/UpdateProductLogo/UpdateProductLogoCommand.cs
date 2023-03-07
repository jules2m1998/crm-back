using CRM.Core.Business.Models.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Product.UpdateProductLogo;

public class UpdateProductLogoCommand: IRequest<ProductOutModel>
{
    public IFormFile Logo { get; set; }
    public Guid Id { get; set; }
    public string UserName { get; set; }

    public UpdateProductLogoCommand(IFormFile logo, Guid id, string userName)
    {
        Logo = logo;
        Id = id;
        UserName = userName;
    }
}
