using Core.Entitys.External;
using Data;
using Infrastructure.Contracts;
using MassTransit;

namespace Infrastructure.Consumers;

class NewProductConsumer : IConsumer<NewProduct>
{
    OrderDbContext _orderDbContext;

    public NewProductConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<NewProduct> context)
    {
        try
        {
            var message = context.Message;

            var product = new ProductVariantCatalogEntry { ProductId = message.ProductID, ProductVariantId = message.ProductVariantId, ProductName = message.ProductName, SKU = message.SKU, UnitPrice = message.UnitPrice };
            await _orderDbContext.ProductVariantCatalogEntrys.InsertOneAsync(product);
        }
        catch
        {
            throw;
        }
    }
}
