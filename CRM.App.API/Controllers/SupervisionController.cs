using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.UseCases.CompanyUseCases.ToggleCompaniesActivation;
using CRM.Core.Business.UseCases.SupervionUCs.AssignSupervisor;
using CRM.Core.Business.UseCases.SupervionUCs.GetAllSupervision;
using CRM.Core.Business.UseCases.SupervionUCs.GetSupervisedByUser;
using CRM.Core.Business.UseCases.SupervionUCs.GetSuperviseesHistory;
using CRM.Core.Business.UseCases.SupervionUCs.GetSupervisionHistory;
using CRM.Core.Business.UseCases.SupervionUCs.GetUserSupervisor;
using CRM.Core.Business.UseCases.SupervionUCs.ToggleSupervisionState;
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

        [HttpPut("ToogleState/{supervisorId:Guid}/{supervisedId:Guid}")]
        [ProducesResponseType(typeof(SupervisionOutModel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ToggleSupervision([FromRoute] Guid supervisorId, [FromRoute] Guid supervisedId)
        {
            var cmd = new ToggleSupervisionStateCommand
            {
                SupervisedId = supervisedId,
                SupervisorId = supervisorId,
                UserName = _username ?? ""
            };

            try
            {
                var result = await _sender.Send(cmd);
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

        [HttpGet("SupervisorOf/{userId:Guid}")]
        [ProducesResponseType(typeof(SupervisionOutModel), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSupervisor([FromRoute] Guid userId)
        {
            var cmd = new GetUserSupervisorCommand
            {
                UserId = userId,
                UserName = _username ?? ""
            };

            try
            {
                var result = await _sender.Send(cmd);
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

        [HttpGet("SupervisedBy/{userId:Guid}")]
        [ProducesResponseType(typeof(ICollection<SupervisionOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSupervised([FromRoute] Guid userId)
        {
            var cmd = new GetSupervisedByUserCommand
            {
                UserId = userId,
                UserName = _username ?? ""
            };

            try
            {
                var result = await _sender.Send(cmd);
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


        [HttpGet("SupervisionHistory/{userId:Guid}")]
        [ProducesResponseType(typeof(ICollection<SupervisionOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSupervisionHistory([FromRoute] Guid userId)
        {
            var cmd = new GetSupervisionHistoryQuery(userId, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
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



        [HttpGet("SuperviseesHistory/{userId:Guid}")]
        [ProducesResponseType(typeof(ICollection<SupervisionOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSuperviseesHistory([FromRoute] Guid userId)
        {
            var cmd = new GetSuperviseesHistoryQuery(userId, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
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



        [HttpGet]
        [ProducesResponseType(typeof(ICollection<SupervisionOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSupervisionQuery(_username ?? "");
            try
            {
                var result = await _sender.Send(query);
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
}
