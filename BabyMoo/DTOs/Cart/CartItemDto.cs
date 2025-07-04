﻿namespace BabyMoo.DTOs.Cart
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
