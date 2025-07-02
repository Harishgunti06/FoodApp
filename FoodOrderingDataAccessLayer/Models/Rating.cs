using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public string Email { get; set; } = null!;

    public int MenuItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? RatingValue { get; set; }

    public int OrderItemId { get; set; }

    public virtual User EmailNavigation { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual OrderItem OrderItem { get; set; } = null!;
}
