using CRM.api.Models;
using CRM.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM.api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IJWTService _jWTService;

        public AuthController(IConfiguration configuration, IJWTService jWTService)
        {
            _configuration = configuration;
            _jWTService = jWTService;
        }

        [HttpPost("/security/createToken")]
        public ActionResult<string> CreateToken(User user)
        {
            if (user.UserName == "j" && user.Password == "j")
            {
                return Ok(_jWTService.GenerateToken(user));
            }
            return Unauthorized();
        }
    }
}
