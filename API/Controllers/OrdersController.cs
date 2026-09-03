using Core.DTOs.Order;
using Core.Entitys;
using Core.Enums;
using Core.ServiceContrast;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/V1/Orders")]
class OrdersController:ControllerBase
{
    IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet]
    public IActionResult GetOrders(
    [FromQuery]Guid? UserId,
    [FromQuery]OrderStatus? Status,
    [FromQuery]OrderAddress? ShippingAddress,
    [FromQuery]DateTime? CreatedAt,
    [FromQuery] int? PageSize=10,[FromQuery]int? PageNum=1,
    [FromQuery] TypeOfSorted? typeOfSorted=TypeOfSorted.ASCENDING,
    [FromQuery] SortOrder? sortOrder=SortOrder.Name)
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetOrderById(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult AddOrder([FromBody] OrderAddRequest orderAddRequest)
    {
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(Guid id,[FromBody] OrderUpdateRequest orderUpdateRequest)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(Guid id)
    {
        return Ok();
    }
}
