using Core.DTOs.OrderAddressSpace;
using Core.Enums;

namespace Core.DTOs.Order;

public class OrderUpdateRequest
{
    public OrderStatus? Status { get; set; }

    public OrderAddressRequest? ShippingAddress { get; set; }

    public OrderAddressRequest? BillingAddress { get; set; }
}
