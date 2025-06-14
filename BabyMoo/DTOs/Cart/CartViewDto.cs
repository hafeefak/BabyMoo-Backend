namespace BabyMoo.DTOs.Cart
{
    public class CartViewDto
    {
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
