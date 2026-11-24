using Core.Entitys;
using Core.Enums;
using Data;
using Infrastructure.Contracts;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public class StockReservationFulfilledConsumer : IConsumer<StockReservationFulfilled>
{
    OrderDbContext _orderDbContext;

    public StockReservationFulfilledConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<StockReservationFulfilled> context)
    {
        var message = context.Message;

        var productID = message.ProductId.Split('#')[0];
        var productVariantId = message.ProductId.Split('#')[1];

        Guid ProductId = Guid.Parse(productID);
        Guid ProductVariantId = Guid.Parse(productVariantId);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.OrderId,message.OrderId),
            Builders<Order>.Filter.ElemMatch(o => o.Items,item => item.ProductId == ProductId &&item.ProductVariantId == ProductVariantId)
        );

        var update = Builders<Order>.Update
            .Set("Items.$.Status", OrderStatus.Confirmed)
            .Set(o => o.UpdatedAt, DateTime.UtcNow)
            .Inc(o => o.OrderVer, 1);

        var result = await _orderDbContext.Orders.UpdateOneAsync(filter, update);
    }
}
