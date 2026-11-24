namespace Core.RedisContrast;

public interface IProductInventoryCache
{
    Task<bool> checkAvailability(Guid ProductId, Guid ProductVariantId, int Qua);
}
