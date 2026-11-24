using Core.Entitys;
using Core.Entitys.External;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Data;

public class OrderDbContext
{
    public IMongoCollection<Order> Orders { get; }
    public IMongoCollection<ProductVariantCatalogEntry> ProductVariantCatalogEntrys { get; }

    public OrderDbContext(
        IMongoDatabase db,
        IOptions<MongoDbSettings> options)
    {
        Console.WriteLine($"options\n ConnectionString: {options.Value.ConnectionString}\n DbName: {options.Value.DbName}\n MongoCollectionNames: {options.Value.CollectionName}");
        var collections = options.Value.CollectionName;

        Orders = db.GetCollection<Order>(
            collections.Order);

        ProductVariantCatalogEntrys = db.GetCollection<ProductVariantCatalogEntry>(
            collections.ProductVariantCatalogEntry);
    }
}
