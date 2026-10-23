using System.Linq.Expressions;
using Core.DTOs.Order;
using Core.DTOs.OrderItems;
using Core.Entitys;
using Core.Entitys.External;
using AutoMapper;
using Core.Enums;
using Core.RedisContrast;
using Core.RepositoryContrast;
using Core.ServiceContrast;
namespace Core.Service;



class OrdersService : IOrdersService
{
    IOrdersRepository _ordersRepository;
    IProductInventoryCache _productInventoryCache;
    IProductVariantCatalogEntryRepository _productVariantCatalogEntryRepository;
    IMapper _mapper;

    public OrdersService(IOrdersRepository ordersRepository, IProductInventoryCache productInventoryCache, IProductVariantCatalogEntryRepository productVariantCatalogEntryRepository, IMapper mapper)
    {
        _ordersRepository = ordersRepository;
        _productInventoryCache = productInventoryCache;
        _productVariantCatalogEntryRepository = productVariantCatalogEntryRepository;
        _mapper = mapper;
    }

    public async Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount)
    {
        return await _ordersRepository.AddNewDiscountAsync(orderId, discount);
    }

    public async Task<bool> AddNewItemAsync(Guid orderId, OrderItemAddRequest request, CancellationToken ct)
    {
        var check = await _productInventoryCache.checkAvailability(orderId, request.Quantity);
        if (!check)
            return false;

        var product = await _productVariantCatalogEntryRepository.GetProductVariantCataById(request.ProductId, request.ProductVariantId);
        return await _ordersRepository.AddNewItemToOrderAsync(orderId, new OrderItem
        {
            ProductId = request.ProductId,
            ProductVariantId = request.ProductVariantId,
            Quantity = request.Quantity,
            UnitPrice = product.UnitPrice,
            ProductName = product.ProductName,
            SKU = product.SKU
        }, ct);
    }

    public async Task<Order?> AddNewOrder(OrderAddRequest request, CancellationToken ct)
    {
        bool check = (await Task.WhenAll(
            request.Items.Select(_ => _productInventoryCache.checkAvailability(_.ProductId, _.Quantity))
        )).All(av => av);

        if (!check) return null;

        var x=Task.WhenAll(
            request.Items.Select(async _ => new OrderItem
            {
                ProductId= _.ProductId,
                ProductVariantId=_.ProductVariantId,
                ProductName=(await _productVariantCatalogEntryRepository.GetProductVariantCataById(_.ProductId, _.ProductVariantId)).ProductName,
            })
        );

        Order order=_mapper.Map<Order>(request);
        order.OrderId=Guid.NewGuid();
        order.Status=OrderStatus.Pending;
        order.CreatedAt=DateTime.Now;


        return await _ordersRepository.CreateOrderAsync(order, ct);
    }

    public async Task<bool> DeleteItemAsync(Guid orderID, OrderItem item, CancellationToken ct)
    {
        return await _ordersRepository.DeleteOrderAsync(orderID, ct);
    }

    public async Task<bool> DeleteOrderAsync(Guid orderID, CancellationToken ct)
    {
        return await _ordersRepository.DeleteOrderAsync(orderID, ct);
    }

    public async Task<Order> GetOrderByOrderIdAsync(Guid orderID)
    {
        return await _ordersRepository.GetOrderByIdAsync(orderID) ?? new Order();
    }

    public async Task<List<Order>> GetOrdersByCondationAsync(Expression<Func<Order, bool>> selector)
    {
        return (await _ordersRepository.GetOrdersByConditionAsync(selector)).ToList();
    }

    public async Task<bool> UpdateTheAddressesOrStatusAsync(Guid orderId, OrderUpdateRequest request) =>
         await _ordersRepository.UpdateOrder(new Order{OrderId=orderId, ShippingAddress= _mapper.Map<OrderAddress>(request.ShippingAddress), BillingAddress= _mapper.Map<OrderAddress>(request.BillingAddress)});
}
