using BabyMoo.DTOs.Payment;
using BabyMoo.Models;

namespace BabyMoo.Services.Payment
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResultDto>> CreatePayment(int orderId);
        Task<ApiResponse<PaymentResultDto>> CapturePayment(string token, string payerId);
    }
}
