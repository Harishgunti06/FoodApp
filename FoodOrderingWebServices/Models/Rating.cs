namespace FoodOrderingWebServices.Models
{
    public class Rating
    {

        public int RatingId { get; set; }

        public string Email { get; set; } = null!;

        public int MenuItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public decimal? RatingValue { get; set; } 
        public int OrderItemId { get; set; }

    }
}
