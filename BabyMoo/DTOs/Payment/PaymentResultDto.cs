namespace BabyMoo.DTOs.Payment
{
    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ApprovalUrl { get; set; } // used in create
        public string Message { get; set; }
    }
}
