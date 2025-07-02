using System.Threading.Tasks;
using Moq;
using FoodOrderingDataAccessLayer;
using Xunit;
using Microsoft.EntityFrameworkCore.Query.Internal;
using FoodOrderingWebServices.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using FoodOrderingDataAccessLayer.Models;
using Microsoft.AspNetCore.Http;

namespace AdminTesting
{
    public class AdminTesting
    {
        private Mock<IFoodRepository> mockRepository;
        private AdminController controller;

        public AdminTesting()
        {
            mockRepository = new Mock<IFoodRepository>();
            controller = new AdminController(mockRepository.Object);
        }

        [Fact]
        public void TestGetAllMenuItems_Success()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            menuItems.Add(new MenuItem() { MenuItemId = 1, ItemName = "Burger", Price = 99 });

            mockRepository
                .Setup(repo => repo.GetAllMenuItems())
                .Returns(menuItems);

            var result = controller.GetAllMenuItems();

            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(menuItems, okResult.Value);
        }

        [Fact]
        public void TestGetAllMenuItems_NotFound()
        {
            mockRepository
                .Setup(repo => repo.GetAllMenuItems())
                .Returns(new List<MenuItem>());

            var result = controller.GetAllMenuItems();

            var notFoundResult = Xunit.Assert.IsType<NotFoundObjectResult>(result);
            Xunit.Assert.Equal("No menu items found.", notFoundResult.Value);
        }

        [Fact]
        public void TestGetAllMenuItems_Exception()
        {
            mockRepository
                .Setup(repo => repo.GetAllMenuItems())
                .Throws(new Exception("Database error"));

            var result = controller.GetAllMenuItems();

            var objectResult = Xunit.Assert.IsType<ObjectResult>(result);
            Xunit.Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task UploadImage_Success()
        {
            var content = "Test image content";
            var fileName = "test.jpg";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFile = new FormFile(ms, 0, ms.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            var upload = new FoodOrderingWebServices.Models.Upload { File = formFile };

            var result = await controller.UploadImage(upload);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            Xunit.Assert.NotNull(dict);
            Xunit.Assert.True(dict.ContainsKey("imageUrl"));
            Xunit.Assert.StartsWith("/images/", dict["imageUrl"]);

        }

        [Fact]
        public async Task UploadImage_ReturnsBadRequest_FileNull()
        {
            // Arrange
            var upload = new FoodOrderingWebServices.Models.Upload { File = null };

            // Act
            var result = await controller.UploadImage(upload);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded", badRequest.Value);
        }

        [Fact]
        public async Task UploadImage_ReturnsBadRequest_FileIsEmpty()
        {
            // Arrange
            var emptyStream = new MemoryStream();
            var formFile = new FormFile(emptyStream, 0, 0, "file", "empty.jpg");

            var upload = new FoodOrderingWebServices.Models.Upload { File = formFile };

            // Act
            var result = await controller.UploadImage(upload);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded", badRequest.Value);
        }

        [Fact]
        public void AddMenuItem_Success()
        {
            // Arrange: API-level model (FoodOrderingWebServices.Models)
            var apiMenuItem = new FoodOrderingWebServices.Models.MenuItem
            {
                ItemName = "Pizza",
                Price = 150,
                CategoryId = 1,
                CuisineId = 2,
                IsVegetarian = true,
                ImageUrl = "/images/pizza.jpg"
            };

            // Expect DAL to succeed
            mockRepository
                .Setup(repo => repo.AddMenuItem(It.IsAny<MenuItem>()))
                .Returns(true);

            // Act
            var result = controller.AddMenuItem(apiMenuItem);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Menu item added successfully.", okResult.Value);
        }

        [Fact]
        public void AddMenuItem_Failure()
        {
            var apiMenuItem = new FoodOrderingWebServices.Models.MenuItem
            {
                ItemName = "Burger",
                Price = 120,
                CategoryId = 1,
                CuisineId = 1,
                IsVegetarian = false,
                ImageUrl = "/images/burger.jpg"
            };

            mockRepository
                .Setup(repo => repo.AddMenuItem(It.IsAny<MenuItem>()))
                .Returns(false);

            var result = controller.AddMenuItem(apiMenuItem);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to Add", badRequestResult.Value);
        }

        [Fact]
        public void AddMenuItem_Exception()
        {
            var apiMenuItem = new FoodOrderingWebServices.Models.MenuItem
            {
                ItemName = "Samosa",
                Price = 20,
                CategoryId = 3,
                CuisineId = 1,
                IsVegetarian = true,
                ImageUrl = "/images/samosa.jpg"
            };

            mockRepository
                .Setup(repo => repo.AddMenuItem(It.IsAny<MenuItem>()))
                .Throws(new Exception("DB connection failed"));

            var result = controller.AddMenuItem(apiMenuItem);

            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("DB connection failed", errorResult.Value.ToString());
        }

        [Fact]
        public void UpdateMenuItem_Success()
        {
            // Arrange
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 1,
                Price = 120
            };

            mockRepository
                .Setup(repo => repo.UpdateMenuItem(apiModel.MenuItemId, apiModel.Price))
                .Returns(true);

            // Act
            var result = controller.UpdateMenuItem(apiModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void UpdateMenuItem_BadRequest_WhenItemIsNull()
        {
            // Act
            var result = controller.UpdateMenuItem(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid menu item data.", badRequest.Value);
        }

        [Fact]
        public void UpdateMenuItem_BadRequest_WhenIdIsInvalid()
        {
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 0,
                Price = 100
            };

            var result = controller.UpdateMenuItem(apiModel);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid menu item data.", badRequest.Value);
        }

        [Fact]
        public void UpdateMenuItem_NotFound_WhenMenuItemDoesNotExist()
        {
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 10,
                Price = 200
            };

            mockRepository
                .Setup(repo => repo.UpdateMenuItem(apiModel.MenuItemId, apiModel.Price))
                .Returns(false);

            var result = controller.UpdateMenuItem(apiModel);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Menu item not found.", notFound.Value);
        }

        [Fact]
        public void DeleteMenuItem_Success()
        {
            // Arrange
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 1,
                ItemName = "Pizza",
                Price = 200
            };

            mockRepository
                .Setup(repo => repo.DeleteMenuItem(It.IsAny<MenuItem>()))
                .Returns(true);

            // Act
            var result = controller.DeleteMenuItem(apiModel);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public void DeleteMenuItem_Failure()
        {
            // Arrange
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 2,
                ItemName = "Burger",
                Price = 120
            };

            mockRepository
                .Setup(repo => repo.DeleteMenuItem(It.IsAny<MenuItem>()))
                .Returns(false);

            // Act
            var result = controller.DeleteMenuItem(apiModel);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value);
        }

        [Fact]
        public void DeleteMenuItem_Exception()
        {
            // Arrange
            var apiModel = new FoodOrderingWebServices.Models.MenuItem
            {
                MenuItemId = 3,
                ItemName = "Sandwich",
                Price = 150
            };

            mockRepository
                .Setup(repo => repo.DeleteMenuItem(It.IsAny<MenuItem>()))
                .Throws(new Exception("Database error"));

            // Act
            var result = controller.DeleteMenuItem(apiModel);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value); // Should return false due to exception
        }

        [Fact]
        public void GetAllOrders_Success()
        {
            // Arrange
            var orders = new List<OrderItem>
            {
                new OrderItem { OrderItemId = 1, Email = "user@example.com", ItemName = "Pizza", Quantity = 2, Price = 200 }
            };

            mockRepository
                .Setup(repo => repo.GetAllOrders())
                .Returns(orders);

            // Act
            var result = controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(orders, okResult.Value);
        }

        [Fact]
        public void GetAllOrders_NotFound()
        {
            // Arrange
            mockRepository
                .Setup(repo => repo.GetAllOrders())
                .Returns(new List<OrderItem>()); // empty list

            // Act
            var result = controller.GetAllOrders();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No orders found.", notFoundResult.Value);
        }

        [Fact]
        public void GetAllOrders_Exception()
        {
            // Arrange
            mockRepository
                .Setup(repo => repo.GetAllOrders())
                .Throws(new Exception("Database error"));

            // Act
            var result = controller.GetAllOrders();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while retrieving orders.", objectResult.Value);
        }

    }
}