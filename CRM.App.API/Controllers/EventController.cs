using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.UseCases.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {
        private readonly ISender _sender;

        public EventController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(EventOutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            var cmd = new GetOneEvents.Query(id, Username);
            return await GetAction(async () => await _sender.Send(cmd));
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventOutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Dictionary<string, ICollection<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] EventInWithoutUserName newEvent)
        {
            EventInModel model = (EventInModel)newEvent;
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
