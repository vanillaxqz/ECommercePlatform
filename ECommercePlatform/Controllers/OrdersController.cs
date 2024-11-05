﻿using Application.DTOs;
using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Commands.OrderCommands;
using Domain.Common;
using Application.UseCases.Queries.OrderQueries;

namespace ECommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetOrderById))]
        public async Task<ActionResult<Result<OrderDto>>> GetOrderById(Guid id)
        {
            var response = await mediator.Send(new GetOrderByIdQuery { Id = id });
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
        [ActionName(nameof(GetOrderByUserId))]
        public async Task<ActionResult<Result<IEnumerable<OrderDto>>>> GetOrderByUserId(Guid id)
        {
            var response = await mediator.Send(new GetOrderByUserIdQuery { Id = id });
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
        public async Task<ActionResult<Result<IEnumerable<OrderDto>>>> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var response = await mediator.Send(query);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Result<Guid>>> CreateOrder(CreateOrderCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return CreatedAtAction("GetOrderById", new { Id = response.Data }, response.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOrderById(Guid id)
        {
            var query = new DeleteOrderCommand { Id = id };
            await mediator.Send(query);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(UpdateOrderCommand update)
        {
            await mediator.Send(update);
            return NoContent();
        }
    }
}
