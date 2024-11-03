using Application.DTOs;
using Application.UseCases.Commands.PaymentCommands;
using Application.UseCases.Queries.PaymentQueries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Controllers
{
    [Route("api/[controller]")]
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
            return await mediator.Send(new GetPaymentByIdQuery { Id = id });
        }
        [HttpGet("User/{id:guid}")]
        [ActionName(nameof(GetPaymentByUserId))]
        public async Task<ActionResult<Result<IEnumerable<PaymentDto>>>> GetPaymentByUserId(Guid id)
        {
            return await mediator.Send(new GetPaymentByUserIdQuery { Id = id });
        }
        [HttpGet]
        public async Task<Result<IEnumerable<PaymentDto>>> GetAllUsers()
        {
            var query = new GetAllPaymentsQuery();
            return await mediator.Send(query);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Result<Guid>>> CreatePayment(CreatePaymentCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction("GetPaymentById", new { Id = id.Data }, id.Data);
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
    }
}
