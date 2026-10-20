using System.Linq.Expressions;
using Core.Entitys;
using Core.Enums;
using Core.RepositoryContrast;
using Data;
using MongoDB.Driver;

namespace Infrastructure.Repository;

public class OrdersRepository : IOrdersRepository
{
    private readonly MongoDbContext _mongoDbContext;

    public OrdersRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }


    private async Task<bool> ApplyUpdateAsync(Guid orderId, UpdateDefinition<Order> update)
    {
        update = update.Set(_ => _.UpdatedAt, DateTime.UtcNow);

        var result = await _mongoDbContext.Orders
            .UpdateOneAsync(_ => _.OrderId == orderId, update);

        return result.IsAcknowledged && result.MatchedCount > 0 && await makeNewVersionOfOrder(orderId);
    }

    public async Task<bool> CreateOrderAsync(Order order)
    {
        await _mongoDbContext.Orders.InsertOneAsync(order);
        return true;
    }

    public async Task<bool> DeleteOrderAsync(Guid orderId)
    {
        var result = await _mongoDbContext.Orders.DeleteOneAsync(_ => _.OrderId == orderId);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOrdersBySpecificConditionAsync(Expression<Func<Order, bool>> predicate)
    {
        var result = await _mongoDbContext.Orders.DeleteManyAsync(predicate);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _mongoDbContext.Orders
            .Find(_ => _.OrderId == orderId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByConditionAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _mongoDbContext.Orders.Find(predicate).ToListAsync();
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var update = Builders<Order>.Update
            .Set(_ => _.Status, newStatus)
            .Push(_ => _.StatusHistory, new OrderStatusHistory
            {
                Status = newStatus,
                ChangedAt = DateTime.UtcNow
            });

        return await ApplyUpdateAsync(orderId, update);
    }

    public async Task<bool> AddNewItemToOrderAsync(Guid orderId, OrderItem item)
    {
        var itemTotal = item.UnitPrice * item.Quantity;

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(_ => _.OrderId, orderId),
            Builders<Order>.Filter.ElemMatch(_ => _.Items,
                _ => _.ProductId == item.ProductId
                  && _.ProductVariantId == item.ProductVariantId));

        var incUpdate = Builders<Order>.Update
            .Inc(_ => _.Items[-1].Quantity, item.Quantity)
            .Inc(_ => _.Items[-1].TotalPrice, itemTotal)
            .Inc(_ => _.SubTotal, itemTotal)
            .Inc(_ => _.Total, itemTotal)
            .Set(_ => _.UpdatedAt, DateTime.UtcNow);

        var result = await _mongoDbContext.Orders.UpdateOneAsync(filter, incUpdate);
        if (result.MatchedCount > 0)
            return true;


        item.TotalPrice = itemTotal;

        var pushUpdate = Builders<Order>.Update
            .Push(_ => _.Items, item)
            .Inc(_ => _.SubTotal, itemTotal)
            .Inc(_ => _.Total, itemTotal);

        return await ApplyUpdateAsync(orderId, pushUpdate);
    }

    public async Task<bool> ChangeShippingAddressAsync(Guid orderId, OrderAddress newAddress)
    {
        var update = Builders<Order>.Update.Set(_ => _.ShippingAddress, newAddress);
        return await ApplyUpdateAsync(orderId, update) ;
    }

    public async Task<bool> ChangeBillingAddressAsync(Guid orderId, OrderAddress newAddress)
    {
        var update = Builders<Order>.Update.Set(_ => _.BillingAddress, newAddress);
        return await ApplyUpdateAsync(orderId, update) ;
    }

    public async Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount)
    {
        if (discount is < 0 or > 100)
            return false;

        var order = await _mongoDbContext.Orders
            .Find(_ => _.OrderId == orderId)
            .FirstOrDefaultAsync();

        if (order is null)
            return false;

        var discountAmount = Math.Round(order.SubTotal * discount / 100, 2);
        var newTotal = order.SubTotal + order.ShippingCost - discountAmount;


        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(_ => _.OrderId, orderId),
            Builders<Order>.Filter.Eq(_=>_.OrderVer, order.OrderVer));

        var update = Builders<Order>.Update
            .Set(_ => _.Discount, discountAmount)
            .Set(_ => _.Total, newTotal)
            .Set(_ => _.UpdatedAt, DateTime.UtcNow)
            .Inc(_=>_.OrderVer, 1);

        var result = await _mongoDbContext.Orders.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount>0 && await makeNewVersionOfOrder(orderId);
    }

    public async Task<bool> makeNewVersionOfOrder(Guid orderId)
    {
        var update=Builders<Order>.Update.Inc(_=>_.OrderVer, 1);
        var result=await _mongoDbContext.Orders.UpdateOneAsync(_=>_.OrderId==orderId,update);
        return result.IsAcknowledged && result.MatchedCount>0;
    }
}
