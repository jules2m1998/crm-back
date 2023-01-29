using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.AddUser;
using CRM.Core.Business.UseCases.AddUsersByCSV;
using CRM.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAdminUser([FromForm] AddAdminUserCommand admin)
        {
            var result = await _sender.Send(admin);
            return Created("", result);
        }

        [HttpPost, Authorize(Roles=Roles.ADMIN)]
        [ProducesResponseType(typeof(List<UserCsvModel>), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUsersByCSV([FromForm] AddUsersByCSVCommand cmd)
        {
            if(cmd.Role == Roles.ADMIN) return Unauthorized();
            try
            {
                var result = await _sender.Send(cmd);
                return Created("", result);
            } catch(UnauthorizedAccessException ex)
            {
                return Unauthorized();
            }
        }
    }
}
