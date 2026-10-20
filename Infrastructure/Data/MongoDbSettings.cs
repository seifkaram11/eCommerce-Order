namespace Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DbName { get; set; } = null!;
    public List<string> CollectionName { get; set; } = new();
}
