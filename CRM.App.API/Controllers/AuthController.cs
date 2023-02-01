using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.GetOneUserByUsername;
using CRM.Core.Business.UseCases.Login;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost, AllowAnonymous]
    [ProducesResponseType(typeof(UserModel), 200)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginQuery login)
    {
        try
        {
            var result = await _sender.Send(login);
            if(result is null) return Unauthorized();
            return Ok(result);
        }catch(UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet, Authorize]
    [ProducesResponseType(typeof(UserModel), 200)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName == null) return Unauthorized();
        GetOneUserByUsernameQuery query = new(userName);
        var result = await _sender.Send(query);
        if (result is null) return Unauthorized();
        return Ok(result);
    }
}
