namespace FoodOrderingWebServices.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        public int CategoryId { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int CuisineId { get; set; }

        public bool IsVegetarian { get; set; }

        public string ImageUrl { get; set; }
    }
}
