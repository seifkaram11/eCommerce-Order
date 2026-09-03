using Core.DTOs.Order;
using FluentValidation;

namespace Core.Validation;

class OrderUpdateRequestValidator:AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid order status.");

        When(x => x.ShippingAddress is not null, () =>
        {
            RuleFor(x => x.ShippingAddress!)
                .SetValidator(new OrderAddressRequestValidator());
        });

        When(x => x.BillingAddress is not null, () =>
        {
            RuleFor(x => x.BillingAddress!)
                .SetValidator(new OrderAddressRequestValidator());
        });
    }
}
