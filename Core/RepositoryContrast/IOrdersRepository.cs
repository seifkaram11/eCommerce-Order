using Core.Entitys;

namespace Core.RepositoryContrast;

public interface IOrdersRepository
{
    Task<bool> CreateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<bool> UpdateOrderStatusAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByConditionAsync(Func <Order, bool> predicate);
    Task<bool> DeleteOrderAsync(Guid ordderId);
    Task<bool> DeleteOrdersBySpecificConditionAsync(Func <Order, bool> predicate);
}
