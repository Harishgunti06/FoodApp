using FoodOrderingDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace FoodOrderingDataAccessLayer
{
    public class FoodRepository : IFoodRepository
    {

        public FoodDbContext Context { get; set; }

        public FoodRepository()
        {
            Context = new FoodDbContext();
        }

        public string ValidateLogin(string email, string password)
        {
            string roleName = "";
            try
            {
                var objUser = (from usr in Context.Users
                               where usr.Email == email && usr.Password == password
                               select usr.Role).FirstOrDefault<Role>();

                if (objUser != null)
                {
                    roleName = objUser.RoleName;
                }
                else
                {
                    roleName = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {

                roleName = "Invalid credentials";
            }
            return roleName;
        }

        public bool RegisterCustomer(User user)
        {
            if (user == null) return false;

            try
            {
                var role = Context.Roles.FirstOrDefault(r => r.RoleName == "Customer");

                if (role == null)
                {
                    Console.WriteLine("Role 'Customer' not found.");
                    return false;
                }

                user.RoleId = role.RoleId; // Assuming `User` has RoleId FK
                                           // OR if Role is navigation property:
                                           // user.Role = role;

                Context.Users.Add(user);
                Context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering customer: {ex.Message}");

                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");

                return false;
            }
        }

        public User GetUserByMail(string email)
        {
            try
            {
                return Context.Users.FirstOrDefault(u => u.Email == email);
            }
            catch
            {
                return null;
            }
        }

        #region Customer

        #region BrowseMenu/ViewItem

        public List<Category> GetAllCategories()
        {
            try
            {
                return Context.Categories.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<Cuisine> GetAllCuisines()
        {
            try
            {
                return Context.Cuisines.ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<MenuItem> GetMenuItemsByCategory(int categoryId)
        {
            try
            {
                return Context.MenuItems.Where(m => m.CategoryId == categoryId).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<MenuItem> GetFilteredMenuItems(MenuFilter filter)
        {
            var query = Context.MenuItems.AsQueryable();

            if (filter.CuisineIds != null && filter.CuisineIds.Any())
            {
                query = query.Where(mi => filter.CuisineIds.Contains(mi.CuisineId));
            }

            if (filter.IsVegetarian.HasValue)
            {
                query = query.Where(mi => mi.IsVegetarian == filter.IsVegetarian.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(mi => mi.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(mi => mi.Price <= filter.MaxPrice.Value);
            }

            return query.ToList();
        }

        #endregion

        #region Add/Update Cart

        public List<CartDetail> GetCartItemsByCustomer(string email)
        {
            try
            {
                return Context.CartDetails.Where(c => c.Email == email).ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool AddToCart(string email, int quantity, string itemName, decimal price)
        {
            try
            {
                var menuItem = Context.MenuItems.FirstOrDefault(m => m.ItemName == itemName);

                if (menuItem == null)
                {
                    Console.WriteLine("MenuItem not found for itemName: " + itemName);
                    return false;
                }

                var cartItem = new CartDetail
                {
                    Email = email,
                    Quantity = quantity,
                    ItemName = menuItem.ItemName,
                    MenuItemId = menuItem.MenuItemId,
                    Price = menuItem.Price
                };

                Context.CartDetails.Add(cartItem);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }

        public bool RemoveCartItem(int menuItemId)
        {
            try
            {
                var item = Context.CartDetails.FirstOrDefault(cd => cd.MenuItemId == menuItemId);
                if (item != null)
                {
                    Context.CartDetails.Remove(item);
                    Context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CheckOut/OrderHistory

        public bool AddOrderItems(List<CartDetail> cartItems)
        {
            try
            {
                foreach (var item in cartItems)
                {
                    OrderItem detail = new OrderItem
                    {
                        MenuItemId = item.MenuItemId,
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        Email = item.Email,
                        Price = item.Price
                    };
                    Context.OrderItems.Add(detail);
                }

                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }

        public List<OrderItem> GetOrdersByCustomer(string email)
        {
            try
            {
                return Context.OrderItems
                       .Where(o => o.Email == email)
                       .ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region ContactSupport
        public bool RaiseIssue(string email, int orderItemId, string issueDescription)
        {
            try
            {
                var issue = new Issue
                {
                    Email = email,
                    OrderItemId = orderItemId,
                    IssueDescription = issueDescription,

                };

                Context.Issues.Add(issue);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }

        #endregion

        #region Profile Manage/Ratings


        public User UpdateUserProfile(User user)
        {
            try
            {
                var userobj = Context.Users.Find(user.Email);
                userobj.PhoneNumber = user.PhoneNumber;
                userobj.Password = user.Password;
                userobj.Address = user.Address;
                Context.SaveChanges();
                return userobj;
            }
            catch
            {
                return null;
            }
        }

        public double? GetAverageRatingByMenuItemId(int menuItemId)
        {
            try
            {
                var averageRating = Context.Ratings
                    .Where(r => r.MenuItemId == menuItemId)
                    .Average(r => (double?)r.RatingValue); // Cast to double? to handle empty sequences

                return averageRating.HasValue ? Math.Round(averageRating.Value, 1) : null;
            }
            catch
            {
                return null;
            }
        }

        public bool AddRating(Rating rating)
        {
            try
            {
                Context.Ratings.Add(rating);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }
        }
        public List<Rating> GetRatingsByemail(string email)
        {
            try
            {
                return Context.Ratings
                    .Where(r => r.Email == email)
                    .ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool AddBooking(Booking booking)
        {
            try
            {
                Context.Bookings.Add(booking);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return false;
            }

        }
        public List<Booking> ViewBookings()
        {
            try
            {
                return Context.Bookings.ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool CheckIn(int bookingId)
        {
            try
            {
                var booking = Context.Bookings.Find(bookingId);
                if (booking is not null)
                {
                    booking.CheckedIn = true;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #endregion

        #region Admin

        #region Manage Menu

        public List<MenuItem> GetAllMenuItems()
        {
            try
            {
                return Context.MenuItems.ToList();
            }
            catch
            {
                return null;
            }
        }

        public bool AddMenuItem(MenuItem item)
        {
            try
            {
                Context.MenuItems.Add(item);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                if (ex.InnerException?.InnerException != null)
                {
                    Console.WriteLine("Nested Inner Exception: " + ex.InnerException.InnerException.Message);
                }
                return false;
            }
        }

        public bool UpdateMenuItem(int menuItemId, decimal price)
        {
            try
            {
                var item = Context.MenuItems.Find(menuItemId);
                if (item != null)
                {
                    item.Price = price;
                    Context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public bool DeleteMenuItem(MenuItem itemss)
        {
            bool status = false;
            try
            {
                bool isReferenced = Context.CartDetails.Any(cd => cd.ItemName == itemss.ItemName);
                if (isReferenced)
                {
                    return false; // or return an error message like "Item is still in use in carts"
                }

                Context.MenuItems.Remove(itemss);
                Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                if (ex.InnerException?.InnerException != null)
                {
                    Console.WriteLine("Nested Inner Exception: " + ex.InnerException.InnerException.Message);
                }
                status = false;
            }
            return status;
        }

        #endregion

        #region Manage Orders

        public List<OrderItem> GetAllOrders()
        {
            try
            {
                return Context.OrderItems.ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region SupportAgent

        #region View Complaints

        public List<Issue> GetAllIssues()
        {
            try
            {
                return Context.Issues.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Issue> GetAllIssuesByEmail(string email)
        {
            try
            {
                return Context.Issues.Where(i => i.Email == email).ToList();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve issues for email '{email}': {ex.Message}");
                return new List<Issue>();
            }
        }

        public bool UpdateIssueStatus(int issueId, string status)
        {
            try
            {
                var issue = Context.Issues.Find(issueId);
                if (issue != null)
                {
                    issue.IssueStatus = status;
                    Context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

    }
}