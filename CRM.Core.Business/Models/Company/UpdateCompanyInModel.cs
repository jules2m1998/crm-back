using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Company;

public class UpdateCompanyInModel
{
    public IFormFile? Logo { get; set; } = null!;
    public IFormFile? CEOPicture { get; set; } = null!;
}
