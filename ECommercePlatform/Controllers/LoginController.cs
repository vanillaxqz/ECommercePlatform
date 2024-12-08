using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Authentication;
using Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Application.UseCases.Commands.UserCommands;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator mediator;
        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
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

        [HttpPost("register")]
        public async Task<ActionResult<Result<Guid>>> RegisterUser(RegisterUserCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Result<Guid>.Success(response.Data);
        }
    }
}