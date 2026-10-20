using System.Linq.Expressions;
using Core.Entitys;
using Core.Enums;

namespace Core.RepositoryContrast;

public interface IOrdersRepository
{
    Task<bool> CreateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(Guid orderId);
    Task<bool> DeleteOrdersBySpecificConditionAsync(Expression<Func<Order, bool>> predicate);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByConditionAsync(Expression<Func<Order, bool>> predicate);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
    Task<bool> AddNewItemToOrderAsync(Guid orderId, OrderItem item);
    Task<bool> ChangeShippingAddressAsync(Guid orderId, OrderAddress newAddress);
    Task<bool> ChangeBillingAddressAsync(Guid orderId, OrderAddress newAddress);
    Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount);
}
