using BabyMoo.Data;
using BabyMoo.Models;            // ✅ for ApiResponse<T>
using BabyMoo.DTOs.Payment;     // ✅ for PaymentResultDto
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _dbContext;
        private readonly PayPalClient _paypalClient;

        public PaymentService(AppDbContext dbContext, PayPalClient paypalClient)
        {
            _dbContext = dbContext;
            _paypalClient = paypalClient;
        }

        public async Task<ApiResponse<PaymentResultDto>> CreatePayment(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return new ApiResponse<PaymentResultDto>(404, "Order not found");

            var paypalOrder = await _paypalClient.CreateOrder(order.TotalAmount);

            order.PaymentToken = paypalOrder.OrderId;
            order.PaymentStatus = "Pending";
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<PaymentResultDto>(200, "Payment started", new PaymentResultDto
            {
                Success = true,
                TransactionId = paypalOrder.OrderId,
                ApprovalUrl = paypalOrder.ApproveLink,
                Message = "Payment created"
            });
        }

        public async Task<ApiResponse<PaymentResultDto>> CapturePayment(string token, string payerId)
        {
            Console.WriteLine($"[PaymentService] Capturing payment for token: {token} and payerId: {payerId}");

            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.PaymentToken == token);
            if (order == null)
                return new ApiResponse<PaymentResultDto>(404, "Order not found");

            var success = await _paypalClient.CaptureOrder(token);

            if (success)
            {
                order.PaymentStatus = "PAID";
                order.Status = "Completed";
                await _dbContext.SaveChangesAsync();

                return new ApiResponse<PaymentResultDto>(200, "Payment captured", new PaymentResultDto
                {
                    Success = true,
                    TransactionId = order.Id.ToString(),
                    Message = "Payment successful"
                });
            }

            return new ApiResponse<PaymentResultDto>(400, "Payment failed", new PaymentResultDto { Success = false });
        }
    }
}
