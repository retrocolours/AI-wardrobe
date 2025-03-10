namespace AI_Wardrobe.Models
{
    public class CartItem
    {
        public string UserId { get; set; } // Store user-specific carts
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
