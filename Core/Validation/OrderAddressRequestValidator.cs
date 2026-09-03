using Core.DTOs.OrderAddressSpace;
using FluentValidation;

namespace Core.Validation;

public class OrderAddressRequestValidator : AbstractValidator<OrderAddressRequest>
{
    public OrderAddressRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(300);

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9\s\-()]{7,20}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Invalid phone number format.");
    }
}
