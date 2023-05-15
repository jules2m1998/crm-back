using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.UseCases.ProspectionUCs.AttributeProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.ChangeProspectionAgent;
using CRM.Core.Business.UseCases.ProspectionUCs.DeleteProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetAgentProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetAllProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetCompanyProspections;
using CRM.Core.Business.UseCases.ProspectionUCs.GetOneProspection;
using CRM.Core.Business.UseCases.ProspectionUCs.GetProspectionByProduct;
using CRM.Core.Business.UseCases.ProspectionUCs.ToggleProspectionsActivationState;
using CRM.Core.Domain;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProspectController : ControllerBase
{
    private readonly ISender _sender;

    public ProspectController(ISender sender)
    {
        _sender = sender;
    }

    private string? Username { get { return User.FindFirstValue(ClaimTypes.Name); } }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ProspectionOutModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _sender.Send(new GetAllProspectionQuery(Username ?? ""));
            return Ok(result);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProspectionOutModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateOne([FromBody] ProspectionInModel model)
    {
        var cmd = new AttributeProspectionCommand(model, Username ?? "");

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

    [HttpGet("{productId:Guid}/{companyId:Guid}/{agentId:Guid}")]
    [ProducesResponseType(typeof(ProspectionOutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOne([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
    {
        var model = new ProspectionInModel(productId, companyId, agentId);
        var query = new GetOneProspectionQuery(model, Username ?? "");
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
        var query = new GetAgentProspectionQuery(agentId, Username ?? "");

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
        var query = new GetCompanyProspectionsQuery(companyId, Username ?? "");

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
        var query = new GetProspectionByProductQuery(productId, Username ?? "");

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
        var cmd = new ToggleProspectionsActivationStateCommand(models, Username ?? "");

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

    [HttpDelete("{productId:Guid}/{companyId:Guid}/{agentId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
    {
        var cmd = new DeleteProspectionCommand(productId, companyId, agentId, Username ?? "");

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

    [HttpPut]
    [ProducesResponseType(typeof(ProspectionOutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] ProspectionInUpdateModel body)
    {
        try
        {
            ChangeProspectionAgentCommand cmd = new(body.ProductId, body.CompanyId, body.AgentId, body.NewAgentId, Username ?? "");
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

    [HttpGet("File"), AllowAnonymous]
    public async Task<IActionResult> File()
    {
        string text = "Hello World";
        byte[] byteArray = Encoding.ASCII.GetBytes(text);

        return File(byteArray, "text/plain", "test.txt");
    }

}
