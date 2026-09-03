using Core.DTOs.Order;
using FluentValidation;

namespace Core.Validation;

class OrderAddRequestValidator:AbstractValidator<OrderAddRequest>
{
    public OrderAddRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Items)
            .NotNull()
            .Must(items => items.Count > 0).WithMessage("At least one order item is required.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemAddRequestValidator());

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("Shipping address is required.")
            .SetValidator(new OrderAddressRequestValidator());

        When(x => x.BillingAddress is not null, () =>
        {
            RuleFor(x => x.BillingAddress!)
                .SetValidator(new OrderAddressRequestValidator());
        });

        RuleFor(x => x.SubTotal)
            .GreaterThanOrEqualTo(0).WithMessage("SubTotal cannot be negative.");

        RuleFor(x => x.ShippingCost)
            .GreaterThanOrEqualTo(0).WithMessage("ShippingCost cannot be negative.");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
            .LessThanOrEqualTo(x => x.SubTotal).WithMessage("Discount cannot exceed SubTotal.");

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0).WithMessage("Total cannot be negative.");

        RuleFor(x => x)
            .Must(x => x.Total == x.SubTotal + x.ShippingCost - x.Discount)
            .WithMessage("Total must equal SubTotal + ShippingCost - Discount.")
            .WithName("Total");
    }
}
