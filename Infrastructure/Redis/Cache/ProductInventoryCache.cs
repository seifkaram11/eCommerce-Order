using Core.RedisContrast;
using Infrastructure.Redis.Data;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public class ProductInventoryCache:IProductInventoryCache
{
    IDatabase _db;
    readonly string prefix="product#";
    string ProductKey(string id)=>prefix+id;

    public ProductInventoryCache(Client db)
    {
        _db = db.GetDatabase();
    }

    public async Task<bool> checkAvailability(Guid ProductId, Guid ProductVariantId, int Qua)
    {
        string key=ProductKey(ProductId+"#"+ProductVariantId);

        int check=(int)await _db.StringGetAsync(key);

        return  check>= Qua;
    }
}
