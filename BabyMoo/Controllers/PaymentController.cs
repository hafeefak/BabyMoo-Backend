using BabyMoo.DTOs.Payment;
using BabyMoo.Models;
using BabyMoo.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

    
        [HttpPost("start/{orderId}")]
        public async Task<ActionResult<ApiResponse<PaymentResultDto>>> Start(int orderId)
        {
            var result = await _paymentService.CreatePayment(orderId);
            return Ok(new ApiResponse<PaymentResultDto>(200, "Payment started", result));
        }

       
        [HttpPost("confirm")]
        public async Task<ActionResult<ApiResponse<PaymentResultDto>>> Confirm([FromQuery] string token)
        {
            var result = await _paymentService.CapturePayment(token);
            return Ok(new ApiResponse<PaymentResultDto>(200, "Payment confirmed", result));
        }
    }
}
