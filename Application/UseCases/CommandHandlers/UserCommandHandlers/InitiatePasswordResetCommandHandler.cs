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

        public InitiatePasswordResetCommandHandler(IUserRepository userRepository, IEmailService emailService,
            JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<string>> Handle(InitiatePasswordResetCommand request,
            CancellationToken cancellationToken)
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

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Your EBUY Password",
                @"
    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; color: #333;'>
        <div style='text-align: center; margin-bottom: 30px;'>
            <h1 style='color: #007bff; font-size: 28px; margin: 0;'>EBUY</h1>
        </div>

        <div style='background-color: #fff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
            <h2 style='margin-top: 0; margin-bottom: 20px; color: #333; font-size: 22px;'>Password Reset Request</h2>
            
            <p style='margin-bottom: 25px; line-height: 1.5; color: #666;'>
                We received a request to reset your EBUY account password. Click the button below to create a new password:
            </p>

            <div style='text-align: center; margin: 30px 0;'>
                <a href='" + resetLink +
                @"' style='display: inline-block; background-color: #007bff; color: #fff; text-decoration: none; padding: 12px 30px; border-radius: 4px; font-weight: 500;'>Reset Password</a>
            </div>

            <p style='margin-bottom: 25px; line-height: 1.5; color: #666;'>
                If you didn't request a password reset, you can safely ignore this email.
            </p>

            <hr style='border: none; border-top: 1px solid #eee; margin: 25px 0;'>

            <p style='margin: 0; font-size: 12px; color: #999; text-align: center;'>
                This link will expire in 15 minutes for security reasons.
            </p>
        </div>

        <div style='text-align: center; margin-top: 20px; color: #999; font-size: 12px;'>
            <p>© " + DateTime.Now.Year + @" EBUY. All rights reserved.</p>
        </div>
    </div>
    ");

            return Result<string>.Success(tokenString);
        }
    }
}