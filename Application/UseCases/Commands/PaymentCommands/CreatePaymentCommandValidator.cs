﻿using FluentValidation;

namespace Application.UseCases.Commands.PaymentCommands
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator() 
        {
            RuleFor(x => x.PaymentDate).NotEmpty();
            RuleFor(x => x.UserId).Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private static bool BeAValidGuid(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                return false;
            }
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
