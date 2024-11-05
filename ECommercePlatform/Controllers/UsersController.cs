using Application.DTOs;
using Application.UseCases.Commands.UserCommands;
using Application.UseCases.Queries.UserQueries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetUserById))]
        public async Task<ActionResult<Result<UserDto>>> GetUserById(Guid id)
        {
            var response = await mediator.Send(new GetUserByIdQuery { Id = id });
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
        public async Task<ActionResult<Result<IEnumerable<UserDto>>>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var response = await mediator.Send(query);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Result<Guid>>> CreateUser(CreateUserCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return CreatedAtAction("GetUserById", new { Id = response.Data }, response.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteById(Guid id)
        {
            var query = new DeleteUserCommand { Id = id };
            await mediator.Send(query);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(UpdateUserCommand update)
        {
            await mediator.Send(update);
            return NoContent();
        }
    }
}
