using FluentValidation;

namespace Application.UseCases.Commands.ProductCommands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Stock).NotEmpty();
        }
    }
}
