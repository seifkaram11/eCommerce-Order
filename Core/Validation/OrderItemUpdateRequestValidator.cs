using Core.DTOs.OrderItems;
using FluentValidation;

namespace Core.Validation;

class OrderItemUpdateRequestValidator:AbstractValidator<OrderItemUpdateRequest>
{
    public OrderItemUpdateRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
