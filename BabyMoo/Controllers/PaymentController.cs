using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BabyMoo.DTOs.Payment;
using BabyMoo.Services.Payment;
using BabyMoo.Models;              // ✅ add this for ApiResponse<T>
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

        // ✅ Starts the PayPal payment → gets approval URL
        [HttpPost("start/{orderId}")]
        public async Task<IActionResult> Start(int orderId)
        {
            var res = await _paymentService.CreatePayment(orderId);
            return StatusCode(res.StatusCode, res);
        }

        // ✅ Confirms/captures payment with token & payerId
        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentRequest request)
        {
            Console.WriteLine($"✅ Received confirm request: token={request?.Token}, payerId={request?.PayerId}");

            if (string.IsNullOrEmpty(request?.Token) || string.IsNullOrEmpty(request?.PayerId))
            {
                Console.WriteLine("❌ Missing token or payerId in request");
                return BadRequest(new ApiResponse<PaymentResultDto>(400, "Token and PayerId are required"));
            }

            var res = await _paymentService.CapturePayment(request.Token, request.PayerId);
            return StatusCode(res.StatusCode, res);
        }
    }

    // ✅ Request DTO for confirm
    public class ConfirmPaymentRequest
    {
        [Required]
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [Required]
        [JsonPropertyName("payerId")]
        public string PayerId { get; set; }
    }
}
