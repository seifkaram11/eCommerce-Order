using Core.Entitys.External;
using Core.RepositoryContrast;
using Data;
using MongoDB.Driver;

namespace Infrastructure.Repository;



public class ProductVariantCatalogEntryRepository
    : IProductVariantCatalogEntryRepository
{
    private readonly OrderDbContext _context;

    public ProductVariantCatalogEntryRepository(
        OrderDbContext context)
    {
        _context = context;
    }

    public async Task<ProductVariantCatalogEntry?>
        GetProductVariantCataById(
            Guid productId,
            Guid productVariantId,
            CancellationToken ct = default)
    {
        var filter = Builders<ProductVariantCatalogEntry>.Filter.And(
            Builders<ProductVariantCatalogEntry>.Filter.Eq(
                x => x.ProductId,
                productId),

            Builders<ProductVariantCatalogEntry>.Filter.Eq(
                x => x.ProductVariantId,
                productVariantId)
        );

        return await _context.ProductVariantCatalogEntrys
            .Find(filter)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<ProductVariantCatalogEntry>>
        GetSomeOfProductVariantCataById(
            List<Tuple<Guid, Guid>> products,
            CancellationToken ct = default)
    {
        if (products is null || products.Count == 0)
            return new List<ProductVariantCatalogEntry>();

        var filters = products.Select(product =>
            Builders<ProductVariantCatalogEntry>.Filter.And(
                Builders<ProductVariantCatalogEntry>.Filter.Eq(
                    x => x.ProductId,
                    product.Item1),

                Builders<ProductVariantCatalogEntry>.Filter.Eq(
                    x => x.ProductVariantId,
                    product.Item2)
            )
        ).ToList();

        var filter = Builders<ProductVariantCatalogEntry>.Filter.Or(
            filters);

        return await _context.ProductVariantCatalogEntrys
            .Find(filter)
            .ToListAsync(ct);
    }
}
