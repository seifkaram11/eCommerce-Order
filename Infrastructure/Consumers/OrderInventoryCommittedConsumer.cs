using Core.Entitys;
using Core.Enums;
using Data;
using Infrastructure.Contracts;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public class OrderInventoryCommittedConsumer : IConsumer<OrderInventoryCommitted>
{
    OrderDbContext _orderDbContext;

    public OrderInventoryCommittedConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<OrderInventoryCommitted> context)
    {
        var message = context.Message;

        var filter = Builders<Order>.Filter.Eq(o => o.OrderId, message.OrderId);

        var update = Builders<Order>.Update
            .Set(o => o.Status, OrderStatus.Confirmed)
            .Set(o => o.UpdatedAt, message.CommittedAt)
            .Inc(o => o.OrderVer, 1);

        var result = await _orderDbContext.Orders.UpdateOneAsync(filter, update);
    }
}
