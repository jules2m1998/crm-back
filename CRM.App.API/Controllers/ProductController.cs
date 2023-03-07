using CRM.Core.Business.Models.Product;
using CRM.Core.Business.UseCases.Product.AddProduct;
using CRM.Core.Business.UseCases.Product.DeleteManyProduct;
using CRM.Core.Business.UseCases.Product.GetAllProduct;
using CRM.Core.Business.UseCases.Product.GetOneProduct;
using CRM.Core.Business.UseCases.Product.UpdateProduct;
using CRM.Core.Business.UseCases.Product.UpdateProductLogo;
using CRM.Core.Domain;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CRM.App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        private string? _username { get { return User.FindFirstValue(ClaimTypes.Name); } }

        [HttpPost, Authorize(Roles =Roles.ADMIN)]
        [ProducesResponseType(typeof(ProductOutModel), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductInModel product)
        {
            var cmd = new AddProductCommand(product, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
                return Created("", result);
            }catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ProductOutModel>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var cmd = new GetAllProductQuery(_username ?? "");
            try
            {
                var results = await _sender.Send(cmd);
                return Ok(results);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(ProductOutModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
        {
            var query = new GetOneProductQuery(_username ?? "", id);
            try
            {
                var result = await _sender.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody] List<Guid> ids)
        {
            var cmd = new DeleteManyProductsCommand(ids, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
                if (result) return NoContent();
                else return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        /**
         * [
              {
                "path": "name",
                "op": "replace",
                "value": "string"
              }
            ]
         */
        [HttpPatch("{id:Guid}")]
        [ProducesResponseType(typeof(ProductOutModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<Product> product)
        {
            var cmd = new UpdateProductCommand(product, id, _username ?? "");
            try
            {
                var result = await _sender.Send(cmd);
                return Ok(result);
            }catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (NotFoundEntityException)
            {
                return NotFound();
            }
        }

        [HttpPut("Logo/{id:Guid}")]
        [ProducesResponseType(typeof(ProductOutModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLogo([FromRoute] Guid id, [Required] IFormFile logo)
        {
            var cmd = new UpdateProductLogoCommand(logo, id, _username ?? "");
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
    }
}
