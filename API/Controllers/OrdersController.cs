using Core.DTOs.Order;
using Core.DTOs.OrderItems;
using Core.Entitys;
using Core.ServiceContrast;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller;

[ApiController]
[Route("api/V1/Orders")]
public class OrdersController:ControllerBase
{
    private readonly IOrdersService _ordersService;
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await _ordersService.GetOrderByOrderIdAsync(id);

        if (order == null)
            return NotFound(new { message = $"Order with ID {id} not found." });

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderAddRequest request, CancellationToken ct)
    {
        var createdOrder = await _ordersService.AddNewOrder(request, ct);

        if (createdOrder is null)
            return BadRequest(new { message = "Failed to create order. Some items may be out of stock." });

        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
    }

    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItemToOrder(Guid id, [FromBody] OrderItemAddRequest request, CancellationToken ct)
    {
        var success = await _ordersService.AddNewItemAsync(id, request, ct );

        if (!success)
            return BadRequest(new { message = "Failed to add item. Check item availability or Order ID." });

        return Ok(new { message = "Item added successfully." });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAddressesOrStatus(Guid id, [FromBody] OrderUpdateRequest request)
    {
        var success = await _ordersService.UpdateTheAddressesOrStatusAsync(id, request);

        if (!success)
            return BadRequest(new { message = "Failed to update order details. Order might not exist." });

        return Ok(new { message = "Order updated successfully." });
    }

    [HttpPut("{id:guid}/discount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDiscount(Guid id, [FromBody] decimal discountPercentage)
    {
        var success = await _ordersService.AddNewDiscountAsync(id, discountPercentage);

        if (!success)
            return BadRequest(new { message = "Invalid discount value or order not found." });

        return Ok(new { message = "Discount applied successfully." });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken ct)
    {
        var success = await _ordersService.DeleteOrderAsync(id , ct);

        if (!success)
            return NotFound(new { message = $"Order with ID {id} not found." });

        return NoContent();
    }
}
