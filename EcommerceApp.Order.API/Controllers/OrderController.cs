using Confluent.Kafka;
using EcommerceApp.Order.Api.Application.Interface;
using EcommerceApp.Order.Api.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommerceApp.Order.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConsumer<string, string> _consumer;
        public OrderController(IOrderService orderService, IConsumer<string, string> consumer)
        {
            _orderService = orderService;
            _consumer = consumer;
        }

        [HttpGet("get-all-orders")]

        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _orderService.GetAllOrderAsync();
            return Ok(result);
        }

        [HttpGet("get-order-by-Id")]

        public async Task<IActionResult> GetOrderByIdAsync(int orderId)
        {
            var result = await _orderService.GetOrderByIdAsync(orderId);

            return Ok(result);
        }

        [HttpPost("create-order")]

        public async Task<IActionResult> CreateOrder([FromQuery] Orders order)
        {
            var result = await _orderService.PlaceOrder(order);

            return CreatedAtAction(nameof(GetOrderByIdAsync),order);
        }

        [HttpGet("place-order")]
        public async Task<IActionResult> ConsumeProductSelection()
        {
            var res = await _orderService.PlaceOrder();
            return Ok(res);
        }

        [HttpPut("update-order")]
        public async Task<IActionResult> UpdateOrder([FromQuery] Orders order)
        {
            var res = await _orderService.UpdateOrderAsync(order);
            return Ok(res);
        }

        [HttpDelete("remove-order")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);
            return NoContent();
        }
    }
}
