using FoodOrderingDataAccessLayer;
using FoodOrderingWebServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWebServices.Controllers
{
    [EnableCors("AllowAngularApp")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IFoodRepository foodrepo;

        public AdminController(IFoodRepository Repo)
        {
            foodrepo = Repo;
        }

        [Authorize]
        [HttpGet("GetAllMenuItems")]
        public IActionResult GetAllMenuItems()
        {
            try
            {
                var menuItems = foodrepo.GetAllMenuItems();
                if (menuItems == null || menuItems.Count == 0)
                    return NotFound("No menu items found.");
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving menu items: {ex.Message}");
            }
        }

        [Authorize(Roles="1")]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] Upload upload)
        {
            var file = upload.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { imageUrl = "/images/" + fileName });
        }

        [Authorize(Roles = "1")]
        [HttpPost("AddMenuItem")]
        public IActionResult AddMenuItem(Models.MenuItem item)
        {
            try
            {
                var menuItem = new FoodOrderingDataAccessLayer.Models.MenuItem
                {
                    ItemName = item.ItemName,
                    Price = item.Price,
                    CategoryId = item.CategoryId,
                    IsVegetarian = item.IsVegetarian,
                    CuisineId = item.CuisineId,
                    ImageUrl = item.ImageUrl  // ✅ Save image URL
                };

                bool result = foodrepo.AddMenuItem(menuItem);
                Console.WriteLine(result);
                if (result)
                    return Ok("Menu item added successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"An error occurred while adding menu item: {ex.Message}");
            }
            return BadRequest("Failed to Add");
        }

        [Authorize(Roles = "1")]
        [HttpPut("UpdateMenuItem")]
        public IActionResult UpdateMenuItem(Models.MenuItem item)
        {
            try
            {
                if (item == null || item.MenuItemId <= 0)
                    return BadRequest("Invalid menu item data.");
                FoodOrderingDataAccessLayer.Models.MenuItem menuItem = new FoodOrderingDataAccessLayer.Models.MenuItem
                {
                    MenuItemId = item.MenuItemId,
                    Price = item.Price,
                };
                bool status = foodrepo.UpdateMenuItem(menuItem.MenuItemId, menuItem.Price);
                if (status)
                    return Ok(true);
                else
                    return NotFound("Menu item not found.");
            }
            catch (Exception ex)
            {

                return BadRequest($"An error occurred while updating menu item: {ex.Message}");
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("DeleteMenuItem")]
        public JsonResult DeleteMenuItem(Models.MenuItem item)
        {
            var status = false;
            FoodOrderingDataAccessLayer.Models.MenuItem itemss = new FoodOrderingDataAccessLayer.Models.MenuItem
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                Price = item.Price
            };
            try
            {
                status = foodrepo.DeleteMenuItem(itemss);
            }
            catch (Exception ex)
            {
                status = false;
                Console.WriteLine(ex.Message);
            }
            return Json(status);
        }

        [Authorize]
        [HttpGet("AllOrders")]
        public ActionResult<List<OrderItem>> GetAllOrders()
        {
            try
            {
                var orders = foodrepo.GetAllOrders();
                if (orders == null || orders.Count == 0)
                    return NotFound("No orders found.");

                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

    }
}
