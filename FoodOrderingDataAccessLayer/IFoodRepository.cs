using FoodOrderingDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingDataAccessLayer
{
    public interface IFoodRepository
    {
        string ValidateLogin(string email, string password);

        bool RegisterCustomer(User user);

        List<MenuItem> GetAllMenuItems();

        bool AddMenuItem(MenuItem item);

        bool UpdateMenuItem(int menuItemId, decimal price);

        bool DeleteMenuItem(MenuItem itemss);

        List<OrderItem> GetAllOrders();

        User GetUserByMail(string email);

        List<Issue> GetAllIssues();

        List<Issue> GetAllIssuesByEmail(string email);

        bool UpdateIssueStatus(int issueId, string status);

        List<Category> GetAllCategories();

        List<Cuisine> GetAllCuisines();

        List<MenuItem> GetMenuItemsByCategory(int categoryId);

        IEnumerable<MenuItem> GetFilteredMenuItems(MenuFilter filter);

        List<CartDetail> GetCartItemsByCustomer(string email);

        bool AddToCart(string email, int quantity, string itemName, decimal price);

        bool RemoveCartItem(int menuItemId);

        bool AddOrderItems(List<CartDetail> cartItems);

        List<OrderItem> GetOrdersByCustomer(string email);

        bool RaiseIssue(string email, int orderItemId, string issueDescription);

        User UpdateUserProfile(User user);

        double? GetAverageRatingByMenuItemId(int menuItemId);

        bool AddRating(Rating rating);

        List<Rating> GetRatingsByemail(string email);

        bool AddBooking(Booking booking);

        List<Booking> ViewBookings();

        bool CheckIn(int bookingId);
    }
}
