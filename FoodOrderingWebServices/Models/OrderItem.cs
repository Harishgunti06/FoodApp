namespace FoodOrderingWebServices.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public string Email { get; set; }
        public int OrderId { get; set; }

        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
