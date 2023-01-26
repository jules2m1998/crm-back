using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.Login;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateToken([FromBody] LoginQuery login)
    {
        var result = await _sender.Send(login);

        if(result is null) return Unauthorized();
        return Ok(result);
    }
}
