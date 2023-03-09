using CRM.Core.Business.Models.Prospect;
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

        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }
        public TestController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne()
        {
            return Ok();
        }
    }
}
