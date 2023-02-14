using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.AddOtherUser;
using CRM.Core.Business.UseCases.AddUser;
using CRM.Core.Business.UseCases.AddUsersByCSV;
using CRM.Core.Business.UseCases.GetUsersByCreator;
using CRM.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddUsersByCSV(
            IFormFile file,
            [FromForm, Required] string role)
        {
            var username = _username;
            var cmd = new AddUsersByCSVCommand
            {
                File = file,
                Role = role,
                CreatorUsername = username!
            };
            if(cmd.Role == Roles.ADMIN) return Unauthorized();
            try
            {
                var result = await _sender.Send(cmd);
                return Created("", result);
            } catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            } catch(BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            } catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.ADMIN},{Roles.SUPERVISOR}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddUser([FromForm] UserBodyAndRole user)
        {
            var username = _username;

            DeserializeSkills(user);

            if (username == null) return Unauthorized();
            var cmd = new AddOtherUserCommand
            {
                User = user,
                CurrentUserName = username
            };
            try
            {
                var result = await _sender.Send(cmd);
                return Created("", result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(List<UserAndCreatorModel>), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllUsers()
        {
            if(_username is null) return Unauthorized();
            var query = new GetUsersByCreatorQuery { CreatorUserName = _username };
            try
            {
                var result = await _sender.Send(query);
                return Ok(result);
            } 
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Deserialize skills and add them to the user request
        /// </summary>
        /// <param name="user"></param>
        private void DeserializeSkills(UserBodyAndRole user)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var studiesForm = Request.Form["Studies"];
            var studies = studiesForm.Select(s => JsonSerializer.Deserialize<SkillModel>(s!, options)).ToList();
            if (studies.Any()) user.Studies = studies!;

            var xpForm = Request.Form["Experiences"];
            var xp = xpForm.Select(s => JsonSerializer.Deserialize<SkillModel>(s!, options)).ToList();
            if (xpForm.Any()) user.Experiences = xp!;
        }
    }
}
