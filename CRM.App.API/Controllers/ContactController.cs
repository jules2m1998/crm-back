using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.UseCases.ContactsUCs.AddContact;
using CRM.Core.Business.UseCases.ContactsUCs.DeleteContact;
using CRM.Core.Business.UseCases.ContactsUCs.GetAllContacts;
using CRM.Core.Business.UseCases.ContactsUCs.GetOneContact;
using CRM.Core.Business.UseCases.ContactsUCs.UpdateContact;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]"), Authorize]
[ApiController]
public class ContactController : BaseController
{
    private readonly ISender _sender;

    public ContactController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactOutModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, ICollection<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Add([FromBody] ContactInModel model)
    {
        var cmd = new AddContactCommand
        {
            UserName = Username ?? "",
            CompanyId = model.CompanyId,
            Name = model.Name,
            Email = model.Email,
            Job = model.Job,
            Phones = model.Phones,
            Visibility = model.Visibility,
            SharedTo = model.SharedTo
        };

        try
        {
            var result = await _sender.Send(cmd);
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

    /// <summary>
    /// Get all current user's contacts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ContactOutModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, ICollection<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var cmd = new GetAllContactsQuery(Username ?? "");
        return await GetAction(async () => await _sender.Send(cmd));
    }

    /// <summary>
    /// Get contact by its id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:Guid}", Name = "GetContact")]
    [ProducesResponseType(typeof(ContactOutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var cmd = new GetOneContactQuery(Username ?? "", id);
        return await GetAction(async () => await _sender.Send(cmd));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteOne([FromRoute] Guid id)
    {
        var cmd = new DeleteContactCommand(id, Username ?? "");
        try
        {
            await _sender.Send(cmd);
            return NoContent();
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

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ContactOutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] ContactInModel model, [FromRoute] Guid id)
    {
        model.Phones = new HashSet<string>(model.Phones);
        var contact = new UpdateContactCommand(id, Username ?? "", model);
        return await GetAction(async () => await _sender.Send(contact));
    }
}
