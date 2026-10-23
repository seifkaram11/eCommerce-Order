using AutoMapper;
using Core.DTOs.OrderAddressSpace;
using Core.Entitys;

namespace Core.Mapping;

class OrderAddressRequestToOrderAddress :Profile
{
    public OrderAddressRequestToOrderAddress()
    {
        CreateMap<OrderAddressRequest, OrderAddress>();
    }
}
