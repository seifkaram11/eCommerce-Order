using Core.DTOs.OrderAddressSpace;
using Core.DTOs.OrderItems;
using Core.DTOs.OrderStatusHistory;
using Core.Entitys;
using Core.Enums;

namespace Core.DTOs.Order;

public class OrderResponse
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public OrderStatus Status { get; set; }

    public List<OrderItemResponse> Items { get; set; } = [];

    public OrderAddressResponse ShippingAddress { get; set; } = null!;

    public OrderAddressResponse? BillingAddress { get; set; }

    public decimal SubTotal { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }

    public List<OrderStatusHistoryResponse> StatusHistory { get; set; } = [];

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
