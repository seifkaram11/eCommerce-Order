namespace Core.Entitys;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }

    public string ProductName { get; set; } = null!;
    public string SKU { get; set; } = null!;

    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}
