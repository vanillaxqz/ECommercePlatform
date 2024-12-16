using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Authentication;
using Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Application.UseCases.Commands.UserCommands;
using Microsoft.AspNetCore.Authorization;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
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

        [HttpPost("initiate-password-reset")]
        public async Task<ActionResult<Result<string>>> InitiatePasswordReset(InitiatePasswordResetCommand command)
        {
            var response = await mediator.Send(command);
            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok(Result<string>.Success(response.Data));
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<ActionResult<Result<string>>> ResetPassword(ResetPasswordCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}