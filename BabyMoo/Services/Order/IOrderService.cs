using BabyMoo.DTOs.Order;
using BabyMoo.Models;

namespace BabyMoo.Services.Orders
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> CreateOrderAsync(int userId, int addressId, CreateOrderDto dto);
        Task<ApiResponse<List<OrderViewDto>>> GetOrders(int userId);
        Task<ApiResponse<List<OrderViewDto>>> GetOrdersforAdmin();
        Task<ApiResponse<int>> TotalProductSold();
        Task<ApiResponse<decimal?>> TotalRevenue();
        Task<ApiResponse<string>> UpdateOrderStatus(int orderId, string newStatus);
    }
}
