using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Security.Claims;

namespace CRM.App.API.Controllers;

public class BaseController : ControllerBase
{
    public string Username => User.FindFirstValue(ClaimTypes.Name) ?? "";

    [NonAction]
    public async Task<IActionResult> GetAction(Func<Task<object>> action)
    {
        try
        {
            var result = await action();
            if (result is Unit)
                return NoContent();
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
