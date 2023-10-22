using CRM.Core.Business.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Services;

public class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetConnectedUserName() => 
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    public Guid? GetUserId()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null) return null;
        return Guid.Parse(id);
    }
}
