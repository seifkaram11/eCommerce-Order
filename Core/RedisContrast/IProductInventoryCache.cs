namespace Core.RedisContrast;

public interface IProductInventoryCache
{
    Task<bool> checkAvailability(Guid id, int Qua);
}
