using Core.DTOs.Order;
using Core.DTOs.OrderItems;
using Core.Entitys;

namespace Core.ServiceContrast;

public interface IOrdersService
{
    Task<bool> UpdateTheAddressesOrStatusAsync(Guid orderId, OrderUpdateRequest request);
    Task<bool> AddNewItemAsync(Guid orderId, OrderItemAddRequest request);
    Task<bool> DeleteItemAsync(Guid orderID, OrderItem item);
    Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount);
    Task<Order> GetOrderByOrderIdAsync(Guid orderID);
    Task<List<Order>> GetOrdersByCondationAsync(Func<Order,bool> selector);
    Task<bool> DeleteOrderAsync(Guid orderID);
    Task<Order> AddNewOrder(OrderAddRequest request);
}
