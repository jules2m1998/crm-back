using CRM.Core.Business.Models.Company;
using CRM.Core.Business.UseCases.CompanyUseCases.AddCompany;
using CRM.Core.Business.UseCases.CompanyUseCases.DeleteManyCompanies;
using CRM.Core.Business.UseCases.CompanyUseCases.GetAllCompanies;
using CRM.Core.Business.UseCases.CompanyUseCases.GetOneCompany;
using CRM.Core.Business.UseCases.CompanyUseCases.PatchCompany;
using CRM.Core.Business.UseCases.CompanyUseCases.ToggleCompaniesActivation;
using CRM.Core.Business.UseCases.CompanyUseCases.UpdateCompanyFiles;
using CRM.Core.Business.UseCases.ToogleAccountActiveted;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ISender _sender;
        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }

        public CompanyController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CompanyOutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CompanyInModel company)
        {
            var cmd = new AddCompanyCommand(company, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
                return CreatedAtAction(nameof(GetOne), new { result.Id }, result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(CompanyOutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            var query = new GetOneCompanyCommand { Id = id, UserName = _username ?? "" };

            try
            {
                var result = await _sender.Send(query);
                return Ok(result);
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<CompanyOutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var cmd = new GetAllCompaniesQuery { UserName = _username ?? "" };
            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPatch("{id:Guid}")]
        [ProducesResponseType(typeof(CompanyOutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<Company> document)
        {
            var cmd = new PatchCompanyCommand { Id= id, JsonPatchDocument= document, UserName= _username ?? "" };
            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("Pictures/{id:Guid}")]
        [ProducesResponseType(typeof(CompanyOutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCompanyFiles([FromForm] UpdateCompanyInModel files, [FromRoute] Guid id)
        {
            var cmd = new UpdateCompanyFilesCommand { Id = id, CompanyFiles = files, UserName = _username ?? "" };

            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("ToggleActivation/{id:Guid}")]
        [ProducesResponseType(typeof(CompanyOutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ToggleActivation([FromRoute] Guid id)
        {
            var ids = new Guid[] { id };
            var cmd = new ToggleCompaniesActivationCommand(ids, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var cmd = new DeleteManyCompaniesCommand(new Guid[] {id}, _username ?? "");

            try
            {
                await _sender.Send(cmd);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
