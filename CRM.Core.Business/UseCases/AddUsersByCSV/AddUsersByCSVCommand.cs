using CRM.Core.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.AddUsersByCSV
{
    public class AddUsersByCSVCommand: IRequest<List<UserCsvModel>>
    {
        [Required]
        public IFormFile File { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        [Required]
        public string CreatorUsername { get; set; }
    }
}
