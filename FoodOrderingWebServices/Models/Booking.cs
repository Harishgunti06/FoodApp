namespace FoodOrderingWebServices.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public string Email { get; set; } = null!;

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? BookingTime { get; set; }

        public int? Guests { get; set; }

        public bool? CheckedIn { get; set; }

    }
}
