using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BabyMoo.Data;
using BabyMoo.DTOs.Order;
using BabyMoo.Models;

namespace BabyMoo.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateOrderAsync(int userId, int addressId, CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = userId,
                AddressId = addressId,
                PaymentStatus = "Pending",
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _dbContext.Products.FindAsync(item.ProductId);
                if (product == null)
                    return new ApiResponse<string>(404, $"Product not found: ID {item.ProductId}");

                if (product.Quantity < item.Quantity)
                    return new ApiResponse<string>(400, $"Not enough stock for product: {product.ProductName}");

               
                product.Quantity -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                totalAmount += product.Price * item.Quantity;
            }

            order.TotalAmount = totalAmount;

            _dbContext.Orders.Add(order);

         
            var userCart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (userCart != null && userCart.CartItems.Any())
            {
                _dbContext.CartItems.RemoveRange(userCart.CartItems);
            }

            await _dbContext.SaveChangesAsync();

            return new ApiResponse<string>(200, "Order created successfully");
        }

        public async Task<ApiResponse<List<OrderViewDto>>> GetOrders(int userId)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Include(o => o.User)
                .ToListAsync();

            var dtoList = _mapper.Map<List<OrderViewDto>>(orders);
            return new ApiResponse<List<OrderViewDto>>(200, "Orders fetched successfully", dtoList);
        }

        public async Task<ApiResponse<List<OrderViewDto>>> GetOrdersforAdmin()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .Include(o => o.User)
                .ToListAsync();

            var dtoList = _mapper.Map<List<OrderViewDto>>(orders);
            return new ApiResponse<List<OrderViewDto>>(200, "All orders fetched", dtoList);
        }

        public async Task<ApiResponse<int>> TotalProductSold()
        {
            var total = await _dbContext.OrderItems.SumAsync(oi => oi.Quantity);
            return new ApiResponse<int>(200, "Total products sold fetched", total);
        }

        public async Task<ApiResponse<decimal?>> TotalRevenue()
        {
            var revenue = await _dbContext.Orders.SumAsync(o => (decimal?)o.TotalAmount);
            return new ApiResponse<decimal?>(200, "Total revenue fetched", revenue);
        }

        public async Task<ApiResponse<string>> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return new ApiResponse<string>(404, "Order not found");

            order.Status = newStatus;
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<string>(200, "Order status updated");
        }
    }
}
