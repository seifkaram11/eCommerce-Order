using Core.RedisContrast;
using Infrastructure.Redis.Data;
using StackExchange.Redis;

namespace Infrastructure.Cache;

public class ProductInventoryCache:IProductInventoryCache
{
    IDatabase _db;
    readonly string prefix="product#";
    string ProductKey(string id)=>prefix+id+":quantity";

    public ProductInventoryCache(Client db)
    {
        _db = db.GetDatabase();
    }

    public async Task<bool> checkAvailability(Guid id, int Qua)
    {
        string key=ProductKey(id.ToString());

        return (int)await _db.StringGetAsync(key) > Qua;
    }
}
