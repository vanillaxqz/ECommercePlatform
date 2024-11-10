using FluentValidation;

namespace Application.UseCases.Commands.UserCommands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(500).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(10);
        }
    }
}
