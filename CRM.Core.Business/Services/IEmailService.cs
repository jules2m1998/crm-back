using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Services;

public interface IEmailService
{
    Task SendAsync(string subject, string body, string receiverEmail);
}
