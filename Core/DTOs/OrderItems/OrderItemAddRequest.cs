namespace Core.DTOs.OrderItems;

public class OrderItemAddRequest
{
    public Guid ProductId { get; set; }

    public Guid ProductVariantId { get; set; }

    public int Quantity { get; set; }
}
