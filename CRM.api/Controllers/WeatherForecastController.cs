using CRM.core.Models;
using CRM.core.UseCases.GetPersons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.api.Controllers
{
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

        [HttpGet(Name = "GetWeatherForecast"), Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "mediatr")]
        [Authorize]
        public async Task<ActionResult<List<Person>>> GetPersonAsync() {
            var person = await _sender.Send(new GetPersonListQuery());

            return person;
        }
    }
}