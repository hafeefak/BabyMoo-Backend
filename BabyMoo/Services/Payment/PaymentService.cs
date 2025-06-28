using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Payment;
using BabyMoo.Middleware;
using BabyMoo.Services.Cart;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICartService _cartServices;
        private readonly IMapper _mapper;

        public PaymentService(AppDbContext dbContext, ICartService cartServices, IMapper mapper)
        {
            _dbContext = dbContext;
            _cartServices = cartServices;
            _mapper = mapper;
        }

        public async Task<PaymentResultDto> CreatePayment(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            var paymentToken = $"PAY-{Guid.NewGuid()}";

            var payment = new Models.Payment
            {
                OrderId = orderId,
                Amount = order.TotalAmount,
                Status = "Pending"
            };
            _dbContext.Payments.Add(payment);

            order.PaymentStatus = "PENDING";
            order.PaymentToken = paymentToken;

            await _dbContext.SaveChangesAsync();

            var paymentDto = new PaymentResultDto
            {
                Success = true,
                TransactionId = paymentToken,
                Message = "Payment created successfully"
            };
            return paymentDto;
        }

        public async Task<PaymentResultDto> CapturePayment(string paymentToken)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.PaymentToken == paymentToken);

            if (order == null)
                throw new NotFoundException("Invalid payment token");

            if (order.PaymentStatus == "PAID")
            {
                return new PaymentResultDto
                {
                    Success = true,
                    TransactionId = paymentToken,
                    Message = "Order already paid"
                };
            }

            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
            if (payment != null)
            {
                payment.Status = "PAID";
            }

            order.PaymentStatus = "PAID";
            order.Status = "Completed"; // ✅ correct: use Status string

            await _cartServices.ClearCart(order.UserId);
            await _dbContext.SaveChangesAsync();

            return new PaymentResultDto
            {
                Success = true,
                TransactionId = paymentToken,
                Message = "Payment successful"
            };
        }
    }
}
