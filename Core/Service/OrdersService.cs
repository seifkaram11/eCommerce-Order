using Core.DTOs.Order;
using Core.DTOs.OrderItems;
using Core.Entitys;
using Core.RepositoryContrast;
using Core.ServiceContrast;

namespace Core.Service;


class OrdersService : IOrdersService
{
    IOrdersRepository _ordersRepository;

    public OrdersService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount)
    {
        return await _ordersRepository.AddNewDiscountAsync(orderId,discount);
    }

    public Task<bool> AddNewItemAsync(Guid orderId, OrderItemAddRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Order> AddNewOrder(OrderAddRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteItemAsync(Guid orderID, OrderItem item)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOrderAsync(Guid orderID)
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderByOrderIdAsync(Guid orderID)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersByCondationAsync(Func<Order, bool> selector)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTheAddressesOrStatusAsync(Guid orderId, OrderUpdateRequest request)
    {
        throw new NotImplementedException();
    }
}
