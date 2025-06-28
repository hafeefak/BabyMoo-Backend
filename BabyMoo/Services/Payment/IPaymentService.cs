using BabyMoo.DTOs.Payment;

namespace BabyMoo.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> CreatePayment(int orderId);
        Task<PaymentResultDto> CapturePayment(string paymentToken);
    }
}
