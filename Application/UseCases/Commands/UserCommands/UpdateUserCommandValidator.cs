using FluentValidation;

namespace Application.UseCases.Commands.UserCommands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(10);
            RuleFor(x => x.UserId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
