using Core.Enums;
using Core.RepositoryContrast;
using Infrastructure.Contracts;
using MassTransit;

namespace Infrastructure.Consumers;

class StockReservationFailedConsumer : IConsumer<StockReservationFailed>
{
    IOrdersRepository _ordersRepository;

    public StockReservationFailedConsumer(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task Consume(ConsumeContext<StockReservationFailed> context)
    {
        await _ordersRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Cancelled);
    }
}
