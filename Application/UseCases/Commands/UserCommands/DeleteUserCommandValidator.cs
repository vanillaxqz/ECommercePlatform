using FluentValidation;

namespace Application.UseCases.Commands.UserCommands
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id).Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private static bool BeAValidGuid(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return false;
            }
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
