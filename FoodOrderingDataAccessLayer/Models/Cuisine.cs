using System;
using System.Collections.Generic;

namespace FoodOrderingDataAccessLayer.Models;

public partial class Cuisine
{
    public int CuisineId { get; set; }

    public string CuisineName { get; set; } = null!;

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
