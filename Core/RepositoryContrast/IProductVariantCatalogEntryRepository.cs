namespace Core.Entitys.External;

public interface IProductVariantCatalogEntryRepository
{
    Task<ProductVariantCatalogEntry> GetProductVariantCataById(Guid ProductID, Guid ProductVariantId);
    Task<ProductVariantCatalogEntry> GetSomeOfProductVariantCataById(List<Tuple<Guid, Guid>> pro);
}
