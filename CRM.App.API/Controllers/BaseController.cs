using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers;

public class BaseController : ControllerBase
{
    public string? Username { get { return User.FindFirstValue(ClaimTypes.Name); } }

    [NonAction]
    public async Task<IActionResult> GetAction(Func<Task<object>> action)
    {
        try
        {
            var result = await action();
            return Ok(result);
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
