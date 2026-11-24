using Core.Entitys.External;
using Data;
using Infrastructure.Contracts;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public class UpdateProductConsumer : IConsumer<UpdateProduct>
{
    OrderDbContext _orderDbContext;

    public UpdateProductConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<UpdateProduct> context)
    {
        try
        {
            var message = context.Message;

            var update=Builders<ProductVariantCatalogEntry>.Update
            .Set(_=>_.ProductName, message.ProductName)
            .Set(_=>_.SKU, message.SKU)
            .Set(_=>_.UnitPrice, message.UnitPrice);

            await _orderDbContext.ProductVariantCatalogEntrys
            .UpdateOneAsync(_ => _.ProductId == message.ProductID
            && _.ProductVariantId==message.ProductVariantId, update);
        }
        catch
        {
            throw;
        }
    }
}
