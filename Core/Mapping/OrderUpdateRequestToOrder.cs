using AutoMapper;
using Core.DTOs.Order;
using Core.Entitys;

namespace Core.Mapping;

class OrderUpdateRequestToOrder:Profile
{
    public OrderUpdateRequestToOrder()
    {
        CreateMap<OrderUpdateRequest, Order>()
        .ForMember(dest=>dest.UserId,op=>op.Ignore())
        .ForMember(dest=>dest.StatusHistory,op=>op.Ignore())
        .ForMember(dest=>dest.CreatedAt,op=>op.Ignore())
        .ForMember(dest=>dest.UpdatedAt,op=>op.Ignore())
        .ForMember(dest=>dest.Items,op=>op.Ignore())
        .ForMember(dest=>dest.SubTotal,op=>op.Ignore())
        .ForMember(dest=>dest.ShippingCost,op=>op.Ignore())
        .ForMember(dest=>dest.Discount,op=>op.Ignore())
        .ForMember(dest=>dest.Total,op=>op.Ignore());
    }
}
