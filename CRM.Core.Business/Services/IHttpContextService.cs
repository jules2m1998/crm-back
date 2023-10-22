using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Services;

public interface IHttpContextService
{
    public string? GetConnectedUserName();
    public Guid? GetUserId();
}
