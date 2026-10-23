using Core.Entitys;
using Core.Entitys.External;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Data;

public class OrderDbContext
{
    public IMongoCollection<Order> Orders { get; set; }
    public IMongoCollection<ProductVariantCatalogEntry> ProductVariantCatalogEntrys { get; set; }
    public OrderDbContext(IMongoDatabase db, IConfiguration configuration)
    {
        Orders = db.GetCollection<Order>(configuration.GetValue<string>("MongoDb:CollectionName:Order"));
        ProductVariantCatalogEntrys=db.GetCollection<ProductVariantCatalogEntry>(configuration.GetValue<string>("MongoDb:CollectionName:ProductVariantCatalogEntry"));
    }
}
