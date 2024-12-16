using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Repositories;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Result<string>.Failure("Invalid token");
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var userResult = await _userRepository.GetUserByIdAsync(userId);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure("User not found");
            }

            var user = userResult.Data;
            user.Password = Hasher.HashPassword(request.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            return Result<string>.Success("Password reset successfully");
        }
    }
}
