using FluentValidation;

namespace Application.UseCases.Commands.OrderCommands
{
    internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator() 
        {
            RuleFor(x => x.OrderDate).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid user guid");
            RuleFor(x => x.PaymentId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid payment guid");
        }
        private static bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
