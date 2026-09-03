using AutoMapper;
using Core.DTOs.Order;
using Core.Entitys;

namespace Core.Mapping;

class OrderAddRequestToOrder:Profile
{
    public OrderAddRequestToOrder()
    {
        CreateMap<OrderAddRequest, Order>()
            .ForMember(dest=>dest.OrderId,op=>op.Ignore())
            .ForMember(dest=>dest.Status,op=>op.Ignore())
            .ForMember(dest=>dest.StatusHistory,op=>op.Ignore())
            .ForMember(dest=>dest.CreatedAt,op=>op.Ignore())
            .ForMember(dest=>dest.UpdatedAt,op=>op.Ignore());
    }
}
