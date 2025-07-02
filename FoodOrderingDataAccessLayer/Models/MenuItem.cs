using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public int CategoryId { get; set; }

    public int CuisineId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsVegetarian { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Category Category { get; set; } = null!;

    public virtual Cuisine Cuisine { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
