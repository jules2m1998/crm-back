using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.UseCases.HeadProspectionUcs;
using CRM.Core.Business.UseCases.HeadProspectionUcs.Commands.MoveHead;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HeadProspectionController : BaseController
    {
        private readonly ISender sender;

        public HeadProspectionController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] HeadProspectionInModel model)
        {
            var command = new AddHeadProspection.Command(model, Username);
            try
            {
                var result = await sender.Send(command);
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
            catch (DuplicateNameException)
            {
                return BadRequest("This head already exist !");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllHeadProspection.Query();
            return Ok(await sender.Send(query));
        }

        [HttpGet]
        [Route("{productId:Guid}/{companyId:Guid}/{agentId:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOne([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
        {
            var query = new GetOneHeadProspection.Query(productId, companyId, agentId);
            try
            {
                var result = await sender.Send(query);
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

        [HttpPut]
        [Route("{productId:Guid}/{companyId:Guid}/{agentId:Guid}/{commitId:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId, [FromRoute] Guid commitId)
        {
            var command = new UpdateHeadProspection.Command(productId, companyId, agentId, commitId);
            try
            {
                var result = await sender.Send(command);
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

        [HttpDelete]
        [Route("{productId:Guid}/{companyId:Guid}/{agentId:Guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
        {
            var command = new DeleteHeadProspection.Command(productId, companyId, agentId);
            try
            {
                var result = await sender.Send(command);
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

        [HttpPost]
        [Route("Move")]
        [ProducesResponseType(typeof(MoveHeadResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Move([FromBody] MoveHeadCommand command)
        {
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("{agentId:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByAgentId([FromRoute] Guid agentId)
        {
            var query = new GetHeadProspectionByAgent.Query(agentId);
            try
            {
                var result = await sender.Send(query);
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
