using Core.DTOs.OrderItems;
using FluentValidation;

namespace Core.Validation;

class OrderItemAddRequestValidator:AbstractValidator<OrderItemAddRequest>
{
    public OrderItemAddRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.ProductVariantId)
            .NotEmpty().WithMessage("ProductVariantId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
