namespace BabyMoo.DTOs.Order
{
    public class OrderViewDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

   
        public string? UserName { get; set; }
        public string? AddressLine { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
