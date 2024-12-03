using Application.DTOs;
using Application.UseCases.Commands.PaymentCommands;
using Application.UseCases.Queries.PaymentQueries;
using Application.Utils;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;
        public PaymentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetPaymentById))]
        public async Task<ActionResult<Result<PaymentDto>>> GetPaymentById(Guid id)
        {
            var response = await mediator.Send(new GetPaymentByIdQuery { Id = id });
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

        [HttpGet("User/{id:guid}")]
        [ActionName(nameof(GetPaymentByUserId))]
        public async Task<ActionResult<Result<IEnumerable<PaymentDto>>>> GetPaymentByUserId(Guid id)
        {
            var response = await mediator.Send(new GetPaymentByUserIdQuery { Id = id });
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
        public async Task<ActionResult<Result<IEnumerable<PaymentDto>>>> GetAllUsers()
        {
            var query = new GetAllPaymentsQuery();
            var response = await mediator.Send(query);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Result<Guid>>> CreatePayment(CreatePaymentCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return CreatedAtAction("GetPaymentById", new { Id = response.Data }, response.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeletePaymentById(Guid id)
        {
            var query = new DeletePaymentCommand { Id = id };
            await mediator.Send(query);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(UpdatePaymentCommand update)
        {
            await mediator.Send(update);
            return NoContent();
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<PaymentDto>>> GetFilteredPayments(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateTime? paymentDate,
            [FromQuery] Guid? userId)
        {
            var query = new GetFilteredPaymentsQuery
            {
                Page = page,
                PageSize = pageSize,
                PaymentDate = paymentDate,
                UserId = userId
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
