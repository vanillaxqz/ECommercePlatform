using FluentValidation;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidator()
        {
            RuleFor(x => x.PaymentDate).NotEmpty();
            RuleFor(x => x.UserId).Must(BeAValidGuid).WithMessage("Must be a valid user guid");
            RuleFor(x => x.PaymentId).Must(BeAValidGuid).WithMessage("Must be a valid payment guid");
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
