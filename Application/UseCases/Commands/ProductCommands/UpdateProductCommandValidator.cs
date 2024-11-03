using FluentValidation;

namespace Application.UseCases.Commands.ProductCommands
{
    internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Stock).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
