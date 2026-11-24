namespace Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DbName { get; set; } = null!;

    public MongoCollectionNames CollectionName { get; set; } = new();
}
//mongodb://root:seifSQLkaram2005%23@localhost:27017/
public record MongoCollectionNames
{
    public string ProductVariantCatalogEntry { get; set; } = null!;

    public string OrderStatusHistory { get; set; } = null!;

    public string OrderItem { get; set; } = null!;

    public string OrderAddress { get; set; } = null!;

    public string Order { get; set; } = null!;
}
