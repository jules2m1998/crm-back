using CRM.Core.Business.UseCases.Test;
using CRM.Core.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.App.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISender _sender;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }


    [HttpGet("GetWeatherForecast"), Authorize(Roles =Roles.ADMIN)]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("mediatr"), Authorize]
    [Authorize]
    public async Task<ActionResult<string[]>> GetPersonAsync()
    {
        var person = await _sender.Send(new GetTestLisQuery());

        return BadRequest();
    }
}