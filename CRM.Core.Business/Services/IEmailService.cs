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
    Task SendAsync(Event e);
    Task SendLastAsync(Event e);
    Task SendSecondAsync(Event e);
}
