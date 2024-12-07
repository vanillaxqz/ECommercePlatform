using Application.DTOs;
using Application.UseCases.Commands.UserCommands;
using Application.UseCases.Queries.UserQueries;
using Application.Utils;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
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
        [Authorize]
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

        [HttpPost("login")]
        public async Task<ActionResult<Result<IList<String>>>> LoginUser(LoginUserCommand user)
        {
            var response = await mediator.Send(user);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            else
            {
                if (response.Data?.Email == null)
                {
                    return BadRequest("Email is required to generate access token.");
                }
                var tokenHandler = new JwtTokenGenerator("3fdd5f93-4ddb-465e-a2e8-3e326175030f");
                var token = tokenHandler.GenerateAccessToken(response.Data.UserId, response.Data.Email);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                List<String> data = [accessToken, response.Data.UserId.ToString()];
                return Result<IList<String>>.Success(data);
            }
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

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetFilteredUsers(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] string? phoneNumber)
        {
            var query = new GetFilteredUsersQuery
            {
                Page = page,
                PageSize = pageSize,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
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
