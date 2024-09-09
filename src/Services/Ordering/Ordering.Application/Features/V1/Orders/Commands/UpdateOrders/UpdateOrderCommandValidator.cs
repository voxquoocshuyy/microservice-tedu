using FluentValidation;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrders;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("{Id} is required.");
        RuleFor(p => p.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required.")
            .NotNull()
            .EmailAddress().WithMessage("{EmailAddress} is not a valid email address.");
        RuleFor(p => p.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required.")
            .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0.");
    }
}