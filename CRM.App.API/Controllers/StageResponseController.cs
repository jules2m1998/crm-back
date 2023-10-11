using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.UseCases.StageResponseUCs;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StageResponseController : BaseController
{
    private readonly ISender sender;
    private readonly IMapper mapper;

    public StageResponseController(ISender sender, IMapper mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }


    [HttpGet]
    [Route("Question/{questionId:Guid}")]
    public async Task<IActionResult> GetOneByQuetion([FromRoute] Guid questionId)
    {
        var query = new GetResponseByStage.Query(questionId);
        return await GetAction(async () => await sender.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StageResponseModelIn data)
    {
        StageResponseModel.In m = data;
        var model = new AddStageResponse.Command(m, Username);
        var response = await sender.Send(model);
        return CreatedAtAction(nameof(GetById), new {response.Id}, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllStageResponse.Query();
        var response = await sender.Send(query);
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var query = new GetStageResponseById.Query(id);
        var response = await sender.Send(query);
        if (response == null) return NotFound();
        return Ok(response);
    }

    [HttpGet]
    [Route("ByHead/{productId:Guid}/{companyId:Guid}/{agentId:Guid}")]
    public async Task<IActionResult> GetByHead([FromRoute] Guid productId, [FromRoute] Guid companyId, [FromRoute] Guid agentId)
    {
        var query = new GetAllByHead.Query(agentId, productId, companyId);
        var response = await sender.Send(query);
        return Ok(response);
    }


    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, StageResponseModelIn response)
    {
        var command = new UpdateStageResponse.Command(id, response);
        var data = await sender.Send(command);
        if(data == null) return NotFound();
        return Ok(data);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteStageResponse.Command(id);
        _ = await sender.Send(command);
        return NoContent();
    }

    public class StageResponseModelIn: StageResponseModel.In
    {

    }
}
