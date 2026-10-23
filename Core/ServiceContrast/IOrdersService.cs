using System.Linq.Expressions;
using Core.DTOs.Order;
using Core.DTOs.OrderItems;
using Core.Entitys;

namespace Core.ServiceContrast;

public interface IOrdersService
{
    Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount);

    Task<bool> AddNewItemAsync(Guid orderId, OrderItemAddRequest request, CancellationToken ct);

    Task<Order?> AddNewOrder(OrderAddRequest request, CancellationToken ct);

    Task<bool> DeleteItemAsync(Guid orderID, OrderItem item, CancellationToken ct);

    Task<bool> DeleteOrderAsync(Guid orderID, CancellationToken ct);

    Task<Order> GetOrderByOrderIdAsync(Guid orderID);

    Task<List<Order>> GetOrdersByCondationAsync(Expression<Func<Order, bool>> selector);

    Task<bool> UpdateTheAddressesOrStatusAsync(Guid orderId, OrderUpdateRequest request);
}
