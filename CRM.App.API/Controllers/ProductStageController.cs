using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.ProductStage;
using CRM.Core.Business.UseCases.StageResponseUCs;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Create([FromBody] IEnumerable<ProductStageModel.In> data)
    {
        var command = new AddProductStage.Command(data, Username);
        try
        {
            var result = await _send.Send(command);
            return Ok(result);
        }catch(NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }catch(UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid id)
    {
        var query = new GetProductStage.Query(id);
        return await GetAction(async () => await _send.Send(query));
    }

    [HttpGet]
    [Route("Product/{id:Guid}")]
    public async Task<IActionResult> GetOneByProduct([FromRoute] Guid id)
    {
        var query = new GetProductStageByProduct.Query(id);
        return await GetAction(async () => await _send.Send(query));
    }

    [HttpGet]
    [Route("First/{productId:Guid}")]
    public async Task<IActionResult> GetFirstByProduct([FromRoute] Guid productId)
    {
        var query = new GetFirstStageByProduct.Query(productId);
        return await GetAction(async () => await _send.Send(query));
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductStage.Query();
        return await GetAction(async () => await _send.Send(query));
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductStageModel.In model)
    {
        var command = new UpdateProductStage.Command(id, model);
        return await GetAction(async () => await _send.Send(command));
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductStage.Command(id);
        return await GetAction(async () => await _send.Send(command));
    }
}
