using Core.Entitys.External;
using Data;
using Infrastructure.Contracts;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public class DeleteProductConsumer : IConsumer<DeleteProduct>
{
    OrderDbContext _orderDbContext;

    public DeleteProductConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<DeleteProduct> context)
    {
        try
        {
            var message = context.Message;
            await _orderDbContext.ProductVariantCatalogEntrys.DeleteOneAsync(_ => _.ProductId == message.ProductID && _.ProductVariantId==message.ProductVariantId);
        }
        catch
        {
            throw;
        }
    }
}
