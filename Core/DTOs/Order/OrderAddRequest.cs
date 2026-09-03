using Core.DTOs.OrderAddressSpace;
using Core.DTOs.OrderItems;
using Core.Entitys;

namespace Core.DTOs.Order;

public class OrderAddRequest
{
    public Guid UserId { get; set; }
    public List<OrderItemAddRequest> Items { get; set; } = [];
    public OrderAddressRequest ShippingAddress { get; set; } = null!;
    public OrderAddressRequest? BillingAddress { get; set; }

    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
}
