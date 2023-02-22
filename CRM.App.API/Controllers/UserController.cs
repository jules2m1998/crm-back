using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.AddOtherUser;
using CRM.Core.Business.UseCases.AddUser;
using CRM.Core.Business.UseCases.AddUsersByCSV;
using CRM.Core.Business.UseCases.GetOneUserById;
using CRM.Core.Business.UseCases.GetUsersByCreator;
using CRM.Core.Business.UseCases.MarkAsDeletedRange;
using CRM.Core.Business.UseCases.ResetPassword;
using CRM.Core.Business.UseCases.UpdateUser;
using CRM.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        [HttpDelete, Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveUsers([FromBody] List<Guid> ids)
        {
            if(_username is null) return Unauthorized();
            if (!ids.Any()) return BadRequest();
            var query = new MarkAsDeletedRangeQuery { Ids= ids, UserName = _username };
            var result = await _sender.Send(query);
            if(result)  return NoContent();
            return BadRequest();
        }

        [HttpPut, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateModel user)
        {
            var cmd = new UpdateUserCommand
            {
                User = user,
                UserName= _username!
            };
            cmd.User.Studies = Deserialize<SkillModel>(Request.Form["Studies"]).ToList();
            cmd.User.Experiences = Deserialize<SkillModel>(Request.Form["Experiences"]).ToList();
            var result = await _sender.Send(cmd);
            if(result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut, Authorize, Route("{id:Guid}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            var cmd = new ResetPasswordCommand { Id= id, UserName = _username ?? "" };
            var result = await _sender.Send(cmd);
            if (result is null) return Unauthorized();
            return Ok(result);
        }

        [HttpGet, Authorize, Route("{id:Guid}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOneUser(Guid id)
        {
            var cmd = new GetOneUserByIdQuery
            {
                Id = id,
                UserName = _username ?? ""
            };
            var result = await _sender.Send(cmd); 
            if (result is null) return NotFound();
            return Ok(result);
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

        private static IEnumerable<T> Deserialize<T>(StringValues values)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return values.Select(v => JsonSerializer.Deserialize<T>(v!, options)!);
        }
    }
}
