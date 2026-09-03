namespace Core.DTOs.OrderAddressSpace;

public class OrderAddressRequest
{
    public string FullName { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}
