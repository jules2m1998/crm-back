using CRM.Core.Business.Authentication;
using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJWTService _jwtService;
    public AuthController(IJWTService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost, AllowAnonymous]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult CreateToken(User u)
    {
        if(u.UserName == "string")
        {
            return Ok(_jwtService.Generate(u));
        }

        return Unauthorized();
    }
}
