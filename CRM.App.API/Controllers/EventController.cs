using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.UseCases.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly ISender _sender;

        public EventController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            var cmd = new GetOneEvents.Query(id, Username);
            return await GetAction(async () => await _sender.Send(cmd));
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] EventInModel model)
        {
            var command = new EditEvent.Command(id, model);
            model.UserName = Username;

            return await GetAction(async () => await _sender.Send(command));
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteEvent.Command(id, Username);
            return await GetAction(async () =>
            {
                await _sender.Send(command);
                return new {};
            });
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var query = new GetEvents.Query(Username);
            return await GetAction(async () => await _sender.Send(query));
        }


        [HttpPost]
        [ProducesResponseType(typeof(Dictionary<string, ICollection<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] EventInWithoutUserName newEvent)
        {
            EventInModel model = new()
            {
                UserName = Username,

                ProductId = newEvent.ProductId,
                CompanyId = newEvent.CompanyId,
                AgentId = newEvent.AgentId,
                OwnerId = newEvent.OwnerId,

                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate,
                Description = newEvent.Description,
                Name = newEvent.Name,
                ContactIds = newEvent.ContactIds,
                Topic = newEvent.Topic
            };
            var query = new AddEvent.Command(model);

            try
            {
                var data = await _sender.Send(query);
                return CreatedAtAction(nameof(GetOne), new {data.Id},data);
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            } catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
