using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Services;
using CRM.Core.Business.UseCases.ProspectionUCs.AttributeProspection;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IEmailService _emailService;

        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }
        public TestController(ISender sender, IEmailService emailService)
        {
            _sender = sender;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> TestEmail()
        {
            await _emailService.SendAsync("Hello", "Hello from CRM", "mevaajules9@gmail.com");
            return NoContent();
        }
    }
}
