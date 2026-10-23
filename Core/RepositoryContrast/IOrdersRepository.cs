using System.Linq.Expressions;
using Core.Entitys;
using Core.Enums;
using MassTransit.MongoDbIntegration;

namespace Core.RepositoryContrast;

public interface IOrdersRepository
{
    Task<Order?> CreateOrderAsync(Order order, CancellationToken ct);
    Task<bool> DeleteOrderAsync(Guid orderId, CancellationToken ct);
    Task<bool> DeleteOrdersBySpecificConditionAsync(Expression<Func<Order, bool>> predicate, CancellationToken ct) ;
    Task<Order?> GetOrderByIdAsync(Guid orderId) ;
    Task<IEnumerable<Order>> GetOrdersByConditionAsync(Expression<Func<Order, bool>> predicate);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
    Task<bool> AddNewItemToOrderAsync(Guid orderId, OrderItem item, CancellationToken ct);
    Task<bool> ChangeShippingAddressAsync(Guid orderId, OrderAddress newAddress) ;
    Task<bool> ChangeBillingAddressAsync(Guid orderId, OrderAddress newAddress);
    Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount);
    Task<bool> UpdateOrder(Order order);
}
