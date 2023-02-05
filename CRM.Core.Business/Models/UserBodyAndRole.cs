using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models
{
    public class UserBodyAndRole: UserBody
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
