using AutoMapper;
using Core.DTOs.OrderItems;
using Core.Entitys;

namespace Core.Mapping;

class OrderItemAddRequestToOrderItem :Profile
{
    public OrderItemAddRequestToOrderItem()
    {
        CreateMap<OrderItemAddRequest, OrderItem>()
        .ForMember(_=>_.ProductName, ac=>ac.Ignore())
        .ForMember(_=>_.SKU, ac=>ac.Ignore())
        .ForMember(_=>_.UnitPrice, ac=>ac.Ignore());
    }
}
