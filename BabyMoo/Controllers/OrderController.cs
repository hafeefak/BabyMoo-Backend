using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Order;
using BabyMoo.Models;

using BabyMoo.Services.Orders;

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) => _orderService = orderService;

      
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrder(int addressId, [FromBody] CreateOrderDto dto)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _orderService.CreateOrderAsync(userId, addressId, dto);
            return StatusCode(result.StatusCode, result);
        }




        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderViewDto>>>> GetMyOrders()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _orderService.GetOrders(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<ActionResult<ApiResponse<List<OrderViewDto>>>> GetAllOrdersForAdmin()
        {
            var result = await _orderService.GetOrdersforAdmin();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            var result = await _orderService.UpdateOrderStatus(orderId, newStatus);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total-products")]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalProductsSold()
        {
            var result = await _orderService.TotalProductSold();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("total-revenue")]
        public async Task<ActionResult<ApiResponse<decimal?>>> GetTotalRevenue()
        {
            var result = await _orderService.TotalRevenue();
            return StatusCode(result.StatusCode, result);
        }
    }
}
