using CRM.Core.Business.UseCases.CommitUcs;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class CommitController : BaseController
    {
        private readonly ISender sender;

        public CommitController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCommit.AddCommitModel model)
        {
            var command = new AddCommit.Command(model, Username);
            try
            {
                var result = await sender.Send(command);
                return CreatedAtAction(nameof(GetOne), new {result.Id}, result);
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
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            var query = new GetOneCommit.Query(id);
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCommits.Query();
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
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] AddCommit.AddCommitModel model, [FromRoute] Guid id)
        {
            var query = new UpdateCommit.Command(model, id);
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

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var query = new DeleteCommit.Command(id);
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
