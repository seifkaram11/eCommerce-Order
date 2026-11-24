using Core.Entitys.External;

namespace Core.RepositoryContrast;

public interface IProductVariantCatalogEntryRepository
{
    Task<ProductVariantCatalogEntry?> GetProductVariantCataById(Guid productId, Guid productVariantId, CancellationToken ct = default);

    Task<List<ProductVariantCatalogEntry>> GetSomeOfProductVariantCataById(List<Tuple<Guid, Guid>> products, CancellationToken ct = default);
}
