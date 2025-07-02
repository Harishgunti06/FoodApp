using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class CartDetail
{
    public int CartId { get; set; }

    public string Email { get; set; } = null!;

    public int MenuItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual User EmailNavigation { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;
}
