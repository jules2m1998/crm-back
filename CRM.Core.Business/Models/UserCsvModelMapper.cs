using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models
{
    public class UserCsvModelMapper: ClassMap<UserCsvModel>
    {
        public UserCsvModelMapper() 
        {
            Map(m => m.UserName).Name("username");
            Map(m => m.Email).Name("email");
            Map(m => m.FirstName).Name("firstname");
            Map(m => m.LastName).Name("lastname");
            Map(m => m.PhoneNumber).Name("phone_number");
        }
    }
}
