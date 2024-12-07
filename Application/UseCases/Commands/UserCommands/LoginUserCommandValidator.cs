using FluentValidation;

namespace Application.UseCases.Commands.UserCommands
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(500).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
