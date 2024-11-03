using FluentValidation;

namespace Application.UseCases.Commands.OrderCommands
{
    internal class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderDate).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid user guid");
            RuleFor(x => x.PaymentId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid payment guid");
            RuleFor(x => x.OrderId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid order guid");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
