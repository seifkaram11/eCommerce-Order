using Core.Enums;

namespace Core.Entitys;

public class OrderStatusHistory
{
    public OrderStatus Status { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Note { get; set; }
}
