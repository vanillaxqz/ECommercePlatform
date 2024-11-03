
using FluentValidation;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
    {
        public DeletePaymentCommandValidator() 
        { 
            RuleFor(x =>  x.Id).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
