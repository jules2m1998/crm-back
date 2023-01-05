using CRM.core.Models;
using CRM.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTService _jWTService;

        public AuthController(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }

        [HttpPost("/security/createToken")]
        public ActionResult<string> CreateToken(User user)
        {
            if (user.UserName == "j")
            {
                return Ok(_jWTService.GenerateToken(user));
            }
            return Unauthorized();
        }
    }
}
