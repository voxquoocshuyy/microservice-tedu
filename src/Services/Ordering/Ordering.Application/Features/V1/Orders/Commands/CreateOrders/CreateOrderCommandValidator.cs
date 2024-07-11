using FluentValidation;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("{UserName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");
        RuleFor(p => p.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required.")
            .NotNull()
            .EmailAddress().WithMessage("{EmailAddress} is not a valid email address.");
        RuleFor(p => p.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required.")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0.");
    }
}