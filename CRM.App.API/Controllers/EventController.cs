using CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;
using CRM.Core.Business.UseCases.EventsUcs.Commands.DeleteEvent;
using CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;
using CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventById;
using CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class EventController : BaseController
{
    private readonly ISender _sender;

    public EventController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("{eventId:Guid}")]
    public async Task<IActionResult> GetEventById([FromRoute] Guid eventId)
    {
        var query = new GetEventByIdQuery
        {
            EventId = eventId
        };
        var result = await _sender.Send(query);
        if(result.Success)
            return Ok(result);
        return NotFound(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddEventCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(AddEventCommandResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddEventCommand newEvent)
    {
        var result = await _sender.Send(newEvent);
        if(!result.Success) return BadRequest(result);

        return CreatedAtAction(nameof(GetEventById), new { eventId = result.Data.Id },result);
    }

    [HttpDelete]
    [Route("{eventId:Guid}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid eventId)
    {
        var command = new DeleteEventCommand
        {
            EventId = eventId
        };
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventCommand updateEvent)
    {
        var result = await _sender.Send(updateEvent);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetMine")]
    public async Task<IActionResult> GetMyEvents()
    {
        var query = new GetEventsByUserQuery();
        var result = await _sender.Send(query);

        return Ok(result);
    }
}
