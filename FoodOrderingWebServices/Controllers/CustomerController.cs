using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodOrderingWebServices.Models;
using FoodOrderingDataAccessLayer;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Http.Logging;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using QRCoder;

namespace FoodOrderingWebServices.Controllers
{
    [EnableCors("AllowAngularApp")]
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class CustomerController : Controller
    {

         IFoodRepository foodRepo;
 
        public CustomerController(IFoodRepository repo)
        {
            foodRepo = repo;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = foodRepo.GetAllCategories();
                if (categories == null || categories.Count == 0)
                {
                    return NotFound("No categories found");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getallcuisines")]
        public IActionResult GetAllCuisines()
        {
            try
            {
                var cuisines = foodRepo.GetAllCuisines();
                if (cuisines == null || cuisines.Count == 0)
                {
                    return NotFound("No cuisines found");
                }
                return Ok(cuisines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMenuItemsByCategory(int categoryId)
        {
            try
            {
                var items = foodRepo.GetMenuItemsByCategory(categoryId);

                if (items == null || items.Count == 0)
                    return NotFound("No menu items found for the selected category.");

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching menu items: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCartItems(string email)
        {
            try
            {
                var cartItems = foodRepo.GetCartItemsByCustomer(email);

                if (cartItems == null || cartItems.Count == 0)
                    return NotFound("No items found in the cart.");

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching cart items: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult AddToCart(string email, int qunatity, string ItemName, decimal Price)
        {
            try
            {
                FoodOrderingDataAccessLayer.Models.CartDetail item = new FoodOrderingDataAccessLayer.Models.CartDetail();
                item.Email = email;
                item.Quantity = qunatity;
                item.ItemName = ItemName;
                item.Price = Price;
                var success = foodRepo.AddToCart(item.Email, item.Quantity, item.ItemName, item.Price);

                if (success)
                    return Ok("Item added to cart successfully.");

                return BadRequest("Failed to add item to cart.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding item to cart: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpDelete("{menuItemId}")]
        public IActionResult RemoveCartItem(int menuItemId)
        {
            try
            {
                var success = foodRepo.RemoveCartItem(menuItemId);

                if (success)
                    return Ok("Cart item removed successfully.");

                return BadRequest("Failed to remove cart item.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while removing cart item: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult AddCartItemsToOrder(List<CartDetail> items)
        {
            try
            {
                if (items == null || !items.Any())
                    return BadRequest("No items provided.");

                var cartItems = items.Select(item => new FoodOrderingDataAccessLayer.Models.CartDetail
                {
                    Email = item.Email,
                    Quantity = item.Quantity,
                    ItemName = item.ItemName,
                    MenuItemId = item.MenuItemId,
                    Price = item.Price

                }).ToList();
                var success = foodRepo.AddOrderItems(cartItems);

                if (success)
                    return Ok(cartItems);

                return BadRequest("Failed to add order items.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding order items: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public IActionResult GetOrdersByCustomer(string email)
        {
            try
            {
                var orders = foodRepo.GetOrdersByCustomer(email);

                if (orders == null || orders.Count == 0)
                    return NotFound("No orders found for the user.");

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving user orders: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult RaiseIssue(string email, int orderItemId, string description)
        {
            try
            {
                FoodOrderingDataAccessLayer.Models.Issue item = new FoodOrderingDataAccessLayer.Models.Issue();
                item.Email = email;
                item.OrderItemId = orderItemId;
                item.IssueDescription = description;
                var success = foodRepo.RaiseIssue(item.Email, item.OrderItemId, item.IssueDescription);

                if (success)
                    return Ok("Issue raised successfully.");

                return BadRequest("Failed to raise the issue.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while raising the issue: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPut]
        public IActionResult UpdateUserProfile(Models.User user)
        {
            try
            {
                FoodOrderingDataAccessLayer.Models.User userToUpdate = new FoodOrderingDataAccessLayer.Models.User
                {

                    FullName = user.FullName,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };
                var users = foodRepo.UpdateUserProfile(userToUpdate);

                if (users != null)
                {
                    return Ok(users);
                }
                else
                    return NotFound("User profile not found.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching user profile: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet]
        public double? DisplayRatingsByItem(int menuItemId)
        {
            try
            {
                var averageRating = foodRepo.GetAverageRatingByMenuItemId(menuItemId);

                if (averageRating == null)
                {
                    return 0;
                }

                return averageRating;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public IActionResult AddRating(Models.Rating rating)
        {
            try
            {
                if (rating == null)
                    return BadRequest("Rating details are required");

                var ratingToAdd = new FoodOrderingDataAccessLayer.Models.Rating
                {
                    MenuItemId = rating.MenuItemId,
                    RatingValue = rating.RatingValue,
                    ItemName = rating.ItemName,
                    Email = rating.Email,
                    OrderItemId = rating.OrderItemId
                };

                var success = foodRepo.AddRating(ratingToAdd);

                if (success)
                    return Ok("Rating added successfully.");

                return BadRequest("Failed to add rating.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding rating: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetRatingDetails(string email)
        {
            try
            {
                var ratings = foodRepo.GetRatingsByemail(email);

                if (ratings == null || !ratings.Any())
                {
                    return NotFound("No ratings found for the provided email.");
                }

                return Ok(ratings); // returns a 200 OK with the list
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching ratings: {ex.Message}");
            }
        }

        [Authorize(Roles = "2")]
        [HttpPost("filter-menu")]
        public IActionResult FilterMenuItems(Models.MenuFilter filter)
        {
            var filterItem = new FoodOrderingDataAccessLayer.Models.MenuFilter
            {
                CuisineIds = filter.CuisineIds,
                IsVegetarian = filter.IsVegetarian,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice
            };
            var filteredItems = foodRepo.GetFilteredMenuItems(filterItem);
            return Ok(filteredItems);
        }

        [Authorize(Roles = "2")]
        [HttpPost("add-booking")]
        public IActionResult AddBooking(Models.Booking booking)
        {
            if (booking == null)
                return BadRequest("Invalid booking data.");
            var bookings = new FoodOrderingDataAccessLayer.Models.Booking
            {
                BookingId=booking.BookingId,
                BookingTime=booking.BookingTime,
                BookingDate=booking.BookingDate,
                Email=booking.Email,
                Guests=booking.Guests,
                CheckedIn=false
            };
            bool result = foodRepo.AddBooking(bookings); // Assuming bookingRepo is injected

            if (result)
                return Ok(bookings);
            else
                return StatusCode(500, "An error occurred while adding the booking.");
        }

        [Authorize]
        [HttpGet("view-booking")]
        public IActionResult ViewBooking()
        {
            var bookings =foodRepo.ViewBookings(); // Assuming bookingRepo is injected

            if (bookings == null || bookings.Count == 0)
                return NotFound("No booking found with the given ID.");

            return Ok(bookings);
        }

        [Authorize]
        [HttpGet("generate-booking-qr")]
        public IActionResult GenerateBookingQR()
        {
            var bookingUrl = "http://localhost:5214/api/Customer/CheckIn/api/bookings/checkin/1";
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(bookingUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var qrImage = qrCode.GetGraphic(20); // Returns base64 string
            return Ok(new { qrImage });
        }

        [Authorize]
        [HttpGet("{bookingId}")]
        public IActionResult CheckIn(int bookingId)
        {
            bool result = foodRepo.CheckIn(bookingId); // Assuming bookingRepo is injected

            if (result)
                return Ok(new { Message = "Checked in successfully." });
            else
                return StatusCode(500, "An error occurred during check-in.");
        }

    }
}
