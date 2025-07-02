using FoodOrderingDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingDataAccessLayer
{
    public interface IFoodInterface
    {
        Task<MenuItem> AddMenuItemAsync(MenuItem item);
    }
}
