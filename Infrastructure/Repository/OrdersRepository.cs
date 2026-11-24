using System.Linq.Expressions;
using Core.Entitys;
using Core.Enums;
using Core.RepositoryContrast;
using Data;
using Infrastructure.Contracts;
using MassTransit;
using MassTransit.MongoDbIntegration;
using MongoDB.Driver;

namespace Infrastructure.Repository;

public class OrdersRepository : IOrdersRepository
{
    private readonly IMongoCollection<Order> _orders;
    private readonly MongoDbContext _mt;
    private readonly IPublishEndpoint _publisher;

    public OrdersRepository(OrderDbContext db, MongoDbContext mt, IPublishEndpoint publisher)
    {
        _orders = db.Orders;
        _mt = mt;
        _publisher = publisher;
    }


    private async Task<T> InTransactionAsync<T>(Func<IClientSessionHandle, Task<T>> work,
    CancellationToken ct)
    {
        await _mt.BeginTransaction(ct);
        try
        {
            var result = await work(_mt.Session!);
            await _mt.CommitTransaction(ct);
            return result;
        }
        catch
        {
            await _mt.Session!.AbortTransactionAsync(CancellationToken.None);
            throw;
        }
    }


    private async Task<bool> ApplyUpdateAsync(Guid orderId, UpdateDefinition<Order> update)
    {
        update = update
            .Set(o => o.UpdatedAt, DateTime.UtcNow)
            .Inc(o => o.OrderVer, 1);

        var result = await _orders.UpdateOneAsync(o => o.OrderId == orderId, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    public Task<Order?> CreateOrderAsync(Order order, CancellationToken ct) =>
        InTransactionAsync<Order?>(async ses =>
        {
            await _orders.InsertOneAsync(ses, order, cancellationToken: ct);

            int expectedItemsCount = order.Items.Count;

            foreach (var item in order.Items)
                await _publisher.Publish(new OrderCretated(order.OrderId, item.ProductId, item.ProductVariantId, order.UserId,item.Quantity, expectedItemsCount, item.TotalPrice), ct);

            return order;
        }, ct);

    public Task<bool> DeleteOrderAsync(Guid orderId, CancellationToken ct) =>
        InTransactionAsync(async ses =>
        {
            var order = await _orders.FindOneAndDeleteAsync(ses, o => o.OrderId == orderId, cancellationToken: ct);
            if (order is null)
                return false;

            foreach (var item in order.Items)
                await _publisher.Publish(
                    new ItemCanceled(orderId, item.ProductId, item.ProductVariantId), ct);

            return true;
        }, ct);

    public Task<bool> DeleteOrdersBySpecificConditionAsync(Expression<Func<Order, bool>> predicate, CancellationToken ct) =>
        InTransactionAsync(async ses =>
        {
            var orders = await _orders.Find(ses, predicate).ToListAsync(ct);
            if (orders.Count == 0)
                return false;

            foreach (var order in orders)
                await _publisher.Publish(new OrderCanceled(order.OrderId));

            var ids = orders.Select(o => o.OrderId).ToList();
            var result = await _orders.DeleteManyAsync(
                ses, Builders<Order>.Filter.In(o => o.OrderId, ids), cancellationToken: ct);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }, ct);

    public async Task<Order?> GetOrderByIdAsync(Guid orderId) =>
        await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();

    public async Task<IEnumerable<Order>> GetOrdersByConditionAsync(Expression<Func<Order, bool>> predicate) =>
        await _orders.Find(predicate).ToListAsync();

    public Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var update = Builders<Order>.Update
            .Set(o => o.Status, newStatus)
            .Push(o => o.StatusHistory, new OrderStatusHistory
            {
                Status = newStatus,
                ChangedAt = DateTime.UtcNow
            });

        return ApplyUpdateAsync(orderId, update);
    }

    public Task<bool> AddNewItemToOrderAsync(Guid orderId, OrderItem item, CancellationToken ct) =>
        InTransactionAsync(async ses =>
        {
            var itemTotal = item.UnitPrice * item.Quantity;
            var now = DateTime.UtcNow;

            var byId = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);
            var hasItem = Builders<Order>.Filter.ElemMatch(o => o.Items,
                i => i.ProductId == item.ProductId && i.ProductVariantId == item.ProductVariantId);

            var incUpdate = Builders<Order>.Update
                .Inc(o => o.Items[-1].Quantity, item.Quantity)
                .Inc(o => o.Items[-1].TotalPrice, itemTotal)
                .Inc(o => o.SubTotal, itemTotal)
                .Set(o => o.UpdatedAt, now)
                .Inc(o => o.OrderVer, 1);

            var incResult = await _orders.UpdateOneAsync(ses, byId & hasItem, incUpdate, cancellationToken: ct);

            if (!incResult.IsAcknowledged || incResult.MatchedCount == 0)
            {
                var pushUpdate = Builders<Order>.Update
                    .Push(o => o.Items, item)
                    .Inc(o => o.SubTotal, itemTotal)
                    .Set(o => o.UpdatedAt, now)
                    .Inc(o => o.OrderVer, 1);

                var pushResult = await _orders.UpdateOneAsync(ses, byId & !hasItem, pushUpdate, cancellationToken: ct);

                if (!pushResult.IsAcknowledged || pushResult.MatchedCount == 0)
                    return false;
            }

            var userId=(await _orders.Find(_=>_.OrderId==orderId).SingleOrDefaultAsync()).UserId;
            await _publisher.Publish(new AddNewItemToOrder(orderId, item.ProductId, item.ProductVariantId, item.Quantity, item.TotalPrice), ct);

            return true;
        }, ct);

    public Task<bool> ChangeShippingAddressAsync(Guid orderId, OrderAddress newAddress) =>
        ApplyUpdateAsync(orderId, Builders<Order>.Update.Set(o => o.ShippingAddress, newAddress));

    public Task<bool> ChangeBillingAddressAsync(Guid orderId, OrderAddress newAddress) =>
        ApplyUpdateAsync(orderId, Builders<Order>.Update.Set(o => o.BillingAddress, newAddress));

    public async Task<bool> AddNewDiscountAsync(Guid orderId, decimal discount)
    {
        if (discount is < 0 or > 100)
            return false;

        var order = await _orders.Find(o => o.OrderId == orderId).FirstOrDefaultAsync();
        if (order is null)
            return false;

        var discountAmount = Math.Round(order.SubTotal * discount / 100, 2);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.OrderId, orderId),
            Builders<Order>.Filter.Eq(o => o.OrderVer, order.OrderVer));

        var update = Builders<Order>.Update
            .Set(o => o.Discount, discountAmount)
            .Set(o => o.UpdatedAt, DateTime.UtcNow)
            .Inc(o => o.OrderVer, 1);

        var result = await _orders.UpdateOneAsync(filter, update);
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        var update=Builders<Order>.Update
            .Set(_=>_.Status, order.Status)
            .Set(_=>_.ShippingAddress, order.ShippingAddress)
            .Set(_=>_.BillingAddress, order.BillingAddress);

        return await ApplyUpdateAsync(order.OrderId, update);
    }
}
