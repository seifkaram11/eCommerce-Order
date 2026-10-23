using Core.Enums;

namespace Core.Entitys;

public class Order
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public OrderStatus Status { get; set; }//*

    public List<OrderItem> Items { get; set; } = [];

    public OrderAddress ShippingAddress { get; set; } = null!;

    public OrderAddress? BillingAddress { get; set; }

    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Discount { get; set; }
    public decimal Total => SubTotal + ShippingCost - (SubTotal * (Discount / 100));

    public List<OrderStatusHistory> StatusHistory { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int OrderVer { get; set; }
}
