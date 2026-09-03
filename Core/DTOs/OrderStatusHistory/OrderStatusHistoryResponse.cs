using Core.Enums;

namespace Core.DTOs.OrderStatusHistory;

public class OrderStatusHistoryResponse
{
    public OrderStatus Status { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? Note { get; set; }
}
