using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly? BookingDate { get; set; }

    public TimeOnly? BookingTime { get; set; }

    public int? Guests { get; set; }

    public bool? CheckedIn { get; set; }

    public virtual User EmailNavigation { get; set; } = null!;
}
