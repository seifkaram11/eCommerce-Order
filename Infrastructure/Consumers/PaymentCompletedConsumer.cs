using Core.Enums;
using Core.RepositoryContrast;
using Infrastructure.Contracts;
using MassTransit;

namespace Infrastructure.Consumers;

public class PaymentCompletedConsumer : IConsumer<PaymentCompleted>
{
    IOrdersRepository _ordersRepository;

    public PaymentCompletedConsumer(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task Consume(ConsumeContext<PaymentCompleted> context)
    {
        await _ordersRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Confirmed);
    }
}
