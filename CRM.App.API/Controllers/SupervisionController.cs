using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.UseCases.SupervionUCs.AssignSupervisor;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SupervisionController : ControllerBase
    {
        private readonly ISender _sender;
        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }

        public SupervisionController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        [ProducesResponseType(typeof(ICollection<SupervisionOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] SupervisionInModel model)
        {
            var cmd = new AssignSupervisorCommand(model, _username ?? "");

            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }catch(NotFoundEntityException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
