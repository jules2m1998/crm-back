using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Product; 
public class ProductInModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public IFormFile? Logo { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
}
