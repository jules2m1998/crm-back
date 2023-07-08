using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.ProductStage;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductStageController : BaseController
{
    private readonly ISender _send;
    public ProductStageController(ISender send)
    {
        _send = send;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductStageModel.Out), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, ICollection<string>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] ProductStageModel.In data)
    {
        var command = new AddProductStage.Command(data, Username);
        try
        {
            var result = await _send.Send(command);
            return CreatedAtAction(nameof(GetOne), new { result.Id }, result);
        }catch(NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }catch(UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ProductStageModel.Out), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var query = new GetProductStage.Query(id);
        return await GetAction(async () => await _send.Send(query));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductStageModel.Out), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductStage.Query();
        return await GetAction(async () => await _send.Send(query));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ProductStageModel.Out), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductStageModel.In model)
    {
        var command = new UpdateProductStage.Command(id, model);
        return await GetAction(async () => await _send.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ProductStageModel.Out), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductStage.Command(id);
        return await GetAction(async () => await _send.Send(command));
    }


}
