using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.UseCases.ProspectionUCs.AttributeProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetAgentProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetCompanyProspections;
using CRM.Core.Business.UseCases.ProspectionUCs.GetOneProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetProspectionByProduct;
using CRM.Core.Business.UseCases.ProspectionUCs.ToggleProspectionsActivationState;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectController : ControllerBase
    {
        private readonly ISender _sender;

        public ProspectController(ISender sender)
        {
            _sender = sender;
        }

        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }

        [HttpPost]
        [ProducesResponseType(typeof(ProspectionOutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOne([FromBody] ProspectionInModel model)
        {
            var cmd = new AttributeProspectionCommand(model, _username ?? "");

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

        [HttpGet("{productId:Guid}/{companyId:Guid}/{agentId:Guid}/")]
        [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOne([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
        {
            var model = new ProspectionInModel(productId, companyId, agentId);
            var query = new GetOneProspectionQuery(model, _username ?? "");
            var result = await _sender.Send(query);

            if (result is null) return NotFound();
            else return Ok(result);
        }

        [HttpGet("AgentProspections/{agentId:Guid}")]
        [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AgentProspections([FromRoute] Guid agentId)
        {
            var query = new GetAgentProspectionQuery(agentId, _username ?? "");

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

        [HttpGet("ByCompany/{companyId:Guid}")]
        [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CompanyProspections([FromRoute] Guid companyId)
        {
            var query = new GetCompanyProspectionsQuery(companyId, _username ?? "");

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



        [HttpGet("ByProduct/{productId:Guid}")]
        [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProductProspections([FromRoute] Guid productId)
        {
            var query = new GetProspectionByProductQuery(productId, _username ?? "");

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

        [HttpPut("ToggleActivationState")]
        [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ToggleState([FromBody] ICollection<ProspectionInModel> models)
        {
            var cmd = new ToggleProspectionsActivationStateCommand(models, _username ?? "");

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
    }
}
