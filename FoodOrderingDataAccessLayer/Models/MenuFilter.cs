using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingDataAccessLayer.Models
{
    public class MenuFilter
    {
        public List<int> CuisineIds { get; set; } = new List<int>(); // IDs of cuisines to filter
        public bool? IsVegetarian { get; set; } // null = both, true = veg, false = non-veg
        public decimal? MinPrice { get; set; } // Optional minimum price
        public decimal? MaxPrice { get; set; } // Optional maximum price
    }
}