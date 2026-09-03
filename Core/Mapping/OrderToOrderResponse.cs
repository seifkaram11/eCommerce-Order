using AutoMapper;
using Core.DTOs.Order;
using Core.Entitys;

namespace Core.Mapping;

class OrderToOrderResponse:Profile
{
    public OrderToOrderResponse()
    {
        CreateMap<Order, OrderResponse>();
    }
}
