using Core.Enums;
using Core.RepositoryContrast;
using Infrastructure.Contracts;
using MassTransit;

namespace Infrastructure.Consumers;

public class PaymentFailedConsumer : IConsumer<PaymentFailed>
{
    IOrdersRepository _ordersRepository;

    public PaymentFailedConsumer(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        await _ordersRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Cancelled);
    }
}
