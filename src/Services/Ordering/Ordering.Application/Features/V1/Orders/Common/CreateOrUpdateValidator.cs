using FluentValidation;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrders;

namespace Ordering.Application.Features.V1.Orders.Common;

public class CreateOrUpdateValidator : AbstractValidator<CreateOrUpdateCommand>
{
    public CreateOrUpdateValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("FirstName is required")
            .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters");
        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("LastName is required")
            .MaximumLength(50).WithMessage("LastName must not exceed 50 characters");
        RuleFor(p => p.EmailAddress)
            .NotEmpty().WithMessage("EmailAddress is required")
            .EmailAddress().WithMessage("A valid EmailAddress is required");
    }
}