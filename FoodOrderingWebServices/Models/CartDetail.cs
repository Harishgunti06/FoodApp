namespace FoodOrderingWebServices.Models
{
    public class CartDetail
    {
        public int CartId { get; set; }

        public string Email { get; set; }

        public int MenuItemId { get; set; }
        public int Price { get; set; }  

        public string ItemName { get; set; }

        public int Quantity { get; set; }
    }
}
