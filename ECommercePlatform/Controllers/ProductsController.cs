using Application.DTOs;
using Application.UseCases.Commands.ProductCommands;
using Application.UseCases.Queries.ProductQueries;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetProductById))]
        public async Task<ActionResult<Result<ProductDto>>> GetProductById(Guid id)
        {
            var response = await mediator.Send(new GetProductByIdQuery { Id = id });
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            if (response.Data == null)
            {
                return NotFound();
            }
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<ProductDto>>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var response = await mediator.Send(query);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Result<Guid>>> CreateProduct(CreateProductCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return CreatedAtAction("GetProductById", new { Id = response.Data }, response.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            var query = new DeleteProductCommand { Id = id };
            await mediator.Send(query);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(UpdateProductCommand update)
        {
            await mediator.Send(update);
            return NoContent();
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetFilteredProducts(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] decimal? price,
            [FromQuery] int? stock,
            [FromQuery] Category? category
        )
        {
            var query = new GetFilteredProductsQuery
            {
                Page = page,
                PageSize = pageSize,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                Category = category
            };
            var response = await mediator.Send(query);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(response.Data);
        }
    }
}
