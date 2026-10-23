namespace Core.Entitys.External;

public class ProductVariantCatalogEntry
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
    public string ProductName { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public decimal UnitPrice { get; set; }
}
