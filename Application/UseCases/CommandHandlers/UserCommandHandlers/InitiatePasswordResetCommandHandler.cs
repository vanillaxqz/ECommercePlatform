using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Repositories;
using Domain.Services;
using Infrastructure;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    public class InitiatePasswordResetCommandHandler : IRequestHandler<InitiatePasswordResetCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public InitiatePasswordResetCommandHandler(IUserRepository userRepository, IEmailService emailService, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<string>> Handle(InitiatePasswordResetCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _userRepository.GetUserByEmailAsync(request.Email);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure("User not found");
            }

            var user = userResult.Data;
            var token = _jwtTokenGenerator.GeneratePasswordResetToken(user.UserId);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var resetLink = $"https://ebuy.digital/reset-password?token={tokenString}";

            await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Click <a href='{resetLink}'>here</a> to reset your password.");

            return Result<string>.Success(tokenString);
        }
    }
}
