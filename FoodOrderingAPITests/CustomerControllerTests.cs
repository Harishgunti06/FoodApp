using FoodOrderingDataAccessLayer;
using FoodOrderingDataAccessLayer.Models;
using FoodOrderingWebServices.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerTesting
{
    public class CustomerControllerTests
    {
        private readonly Mock<IFoodRepository> _mockRepository;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockRepository = new Mock<IFoodRepository>();
            _controller = new CustomerController(_mockRepository.Object);
        }

        [Fact]
        public void GetAllCategories_ReturnsOkWithCategories()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCategories()).Returns(new List<FoodOrderingDataAccessLayer.Models.Category>
            {
                new FoodOrderingDataAccessLayer.Models.Category { CategoryId = 1, CategoryName = "Starters" }
            });

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<List<FoodOrderingDataAccessLayer.Models.Category>>(okResult.Value);
            Assert.Single(categories);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void GetAllCategories_ReturnsNotFound_WhenNoCategories()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCategories()).Returns(new List<FoodOrderingDataAccessLayer.Models.Category>());

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCategories();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No categories found", notFoundResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetAllCategories_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCategories()).Throws(new Exception("DB error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCategories();

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
            Assert.Equal("DB error", serverError.Value);
        }
        [Fact]
        public void GetAllCuisines_ReturnsOk_WhenDataExists()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCuisines()).Returns(new List<Cuisine>
            {
                new Cuisine { CuisineId = 1, CuisineName = "Indian" }
            });

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCuisines();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<List<Cuisine>>(okResult.Value);
            Assert.Single(data);
            Assert.Equal("Indian", data[0].CuisineName);
        }

        [Fact]
        public void GetAllCuisines_ReturnsNotFound_WhenListIsEmpty()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCuisines()).Returns(new List<Cuisine>());

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCuisines();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No cuisines found", notFoundResult.Value);
        }

        [Fact]
        public void GetAllCuisines_ReturnsServerError_WhenExceptionThrown()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAllCuisines()).Throws(new Exception("DB error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetAllCuisines();

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("DB error", errorResult.Value);
        }
        [Fact]
        public void GetMenuItemsByCategory_ReturnsOk_WhenItemsExist()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetMenuItemsByCategory(1)).Returns(new List<FoodOrderingDataAccessLayer.Models.MenuItem>
            {
                new FoodOrderingDataAccessLayer.Models.MenuItem { MenuItemId = 1, ItemName = "Paneer Tikka", CategoryId = 1 }
            });

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetMenuItemsByCategory(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsType<List<FoodOrderingDataAccessLayer.Models.MenuItem>>(okResult.Value);
            Assert.Single(items);
            Assert.Equal("Paneer Tikka", items[0].ItemName);
        }

        [Fact]
        public void GetMenuItemsByCategory_ReturnsNotFound_WhenNoItems()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetMenuItemsByCategory(2)).Returns(new List<FoodOrderingDataAccessLayer.Models.MenuItem>());

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetMenuItemsByCategory(2);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No menu items found for the selected category.", notFoundResult.Value);
        }

        [Fact]
        public void GetMenuItemsByCategory_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetMenuItemsByCategory(It.IsAny<int>()))
                    .Throws(new Exception("DB failure"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetMenuItemsByCategory(99);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("An error occurred while fetching menu items: DB failure", errorResult.Value);
        }
        [Fact]
        public void GetCartItems_ReturnsOk_WhenItemsExist()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetCartItemsByCustomer("user@example.com"))
                    .Returns(new List<FoodOrderingDataAccessLayer.Models.CartDetail>
                    {
                        new FoodOrderingDataAccessLayer.Models.CartDetail {  Email = "user@example.com", Quantity = 2 }
                    });

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetCartItems("user@example.com");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsType<List<FoodOrderingDataAccessLayer.Models.CartDetail>>(okResult.Value);
            Assert.Single(items);
            Assert.Equal("user@example.com", items[0].Email);
        }

        [Fact]
        public void GetCartItems_ReturnsNotFound_WhenCartIsEmpty()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetCartItemsByCustomer("empty@example.com"))
                    .Returns(new List<FoodOrderingDataAccessLayer.Models.CartDetail>());

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetCartItems("empty@example.com");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No items found in the cart.", notFound.Value);
        }

        [Fact]
        public void GetCartItems_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetCartItemsByCustomer(It.IsAny<string>()))
                    .Throws(new Exception("DB failure"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetCartItems("error@example.com");

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
            Assert.Equal("An error occurred while fetching cart items: DB failure", serverError.Value);
        }
        [Fact]
        public void AddToCart_ReturnsOk_WhenAddSucceeds()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddToCart("user@example.com", 2, "Burger", 120.00m))
                    .Returns(true);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddToCart("user@example.com", 2, "Burger", 120.00m);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Item added to cart successfully.", okResult.Value);
        }

        [Fact]
        public void AddToCart_ReturnsBadRequest_WhenAddFails()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddToCart("user@example.com", 1, "Pizza", 200.00m))
                    .Returns(false);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddToCart("user@example.com", 1, "Pizza", 200.00m);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add item to cart.", badRequestResult.Value);
        }

        [Fact]
        public void AddToCart_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddToCart(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>()))
                    .Throws(new Exception("DB failure"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddToCart("user@example.com", 1, "Pizza", 200.00m);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("An error occurred while adding item to cart: DB failure", errorResult.Value);
        }
        [Fact]
        public void RemoveCartItem_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RemoveCartItem(1)).Returns(true);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RemoveCartItem(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Cart item removed successfully.", okResult.Value);
        }

        [Fact]
        public void RemoveCartItem_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RemoveCartItem(2)).Returns(false);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RemoveCartItem(2);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to remove cart item.", badRequestResult.Value);
        }

        [Fact]
        public void RemoveCartItem_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RemoveCartItem(It.IsAny<int>()))
                    .Throws(new Exception("Database error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RemoveCartItem(99);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("An error occurred while removing cart item: Database error", errorResult.Value);
        }
        [Fact]
        public void AddCartItemsToOrder_ReturnsOk_WhenItemsAreAdded()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            var items = new List<FoodOrderingWebServices.Models.CartDetail>
            {
                new FoodOrderingWebServices.Models.CartDetail { Email = "user@example.com", Quantity = 2, ItemName = "Pizza", MenuItemId = 1, Price = 199 }
            };

            mockRepo.Setup(repo => repo.AddOrderItems(It.IsAny<List<FoodOrderingDataAccessLayer.Models.CartDetail>>())).Returns(true);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddCartItemsToOrder(items);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsType<List<FoodOrderingDataAccessLayer.Models.CartDetail>>(okResult.Value);
            Assert.Single(returnedItems);
            Assert.Equal("Pizza", returnedItems[0].ItemName);
        }

        [Fact]
        public void AddCartItemsToOrder_ReturnsBadRequest_WhenItemsIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddCartItemsToOrder(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No items provided.", badRequest.Value);
        }

        [Fact]
        public void AddCartItemsToOrder_ReturnsBadRequest_WhenAddFails()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            var items = new List<FoodOrderingWebServices.Models.CartDetail>
            {
                new FoodOrderingWebServices.Models.CartDetail { Email = "user@example.com", Quantity = 1, ItemName = "Burger", MenuItemId = 2, Price = 99 }
            };

            mockRepo.Setup(repo => repo.AddOrderItems(It.IsAny<List<FoodOrderingDataAccessLayer.Models.CartDetail>>())).Returns(false);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddCartItemsToOrder(items);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add order items.", badRequest.Value);
        }

        [Fact]
        public void AddCartItemsToOrder_ReturnsServerError_OnException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddOrderItems(It.IsAny<List<FoodOrderingDataAccessLayer.Models.CartDetail>>()))
                    .Throws(new Exception("DB failure"));

            var controller = new CustomerController(mockRepo.Object);
            var items = new List<FoodOrderingWebServices.Models.CartDetail>
            {
                new FoodOrderingWebServices.Models.CartDetail { Email = "user@example.com", Quantity = 1, ItemName = "Burger", MenuItemId = 2, Price = 99 }
            };

            // Act
            var result = controller.AddCartItemsToOrder(items);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("An error occurred while adding order items", errorResult.Value.ToString());
        }
        [Fact]
        public void GetOrdersByCustomer_ReturnsOk_WhenOrdersExist()
        {
            // Arrange
            var email = "user@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetOrdersByCustomer(email))
                    .Returns(new List<FoodOrderingDataAccessLayer.Models.OrderItem>
                    {
                        new FoodOrderingDataAccessLayer.Models.OrderItem { OrderItemId = 1, Email = email, Price = 299.99m }
                    });

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetOrdersByCustomer(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsType<List<FoodOrderingDataAccessLayer.Models.OrderItem>>(okResult.Value);
            Assert.Single(orders);
        }

        [Fact]
        public void GetOrdersByCustomer_ReturnsNotFound_WhenNoOrdersExist()
        {
            // Arrange
            var email = "user@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetOrdersByCustomer(email))
                    .Returns(new List<FoodOrderingDataAccessLayer.Models.OrderItem>()); // empty

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetOrdersByCustomer(email);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found for the user.", notFound.Value);
        }

        [Fact]
        public void GetOrdersByCustomer_ReturnsServerError_OnException()
        {
            // Arrange
            var email = "user@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetOrdersByCustomer(email))
                    .Throws(new Exception("Database error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetOrdersByCustomer(email);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("An error occurred while retrieving user orders", errorResult.Value.ToString());
        }
        [Fact]
        public void RaiseIssue_ReturnsOk_WhenIssueIsRaisedSuccessfully()
        {
            // Arrange
            var email = "user@example.com";
            var orderItemId = 101;
            var description = "Item was cold";

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RaiseIssue(email, orderItemId, description)).Returns(true);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RaiseIssue(email, orderItemId, description);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Issue raised successfully.", okResult.Value);
        }

        [Fact]
        public void RaiseIssue_ReturnsBadRequest_WhenRaiseFails()
        {
            // Arrange
            var email = "user@example.com";
            var orderItemId = 102;
            var description = "Wrong item delivered";

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RaiseIssue(email, orderItemId, description)).Returns(false);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RaiseIssue(email, orderItemId, description);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to raise the issue.", badRequest.Value);
        }

        [Fact]
        public void RaiseIssue_ReturnsServerError_OnException()
        {
            // Arrange
            var email = "user@example.com";
            var orderItemId = 103;
            var description = "Did not receive the item";

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.RaiseIssue(email, orderItemId, description))
                    .Throws(new Exception("Database error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.RaiseIssue(email, orderItemId, description);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("An error occurred while raising the issue", errorResult.Value.ToString());
        }
        [Fact]
        public void UpdateUserProfile_ReturnsOk_WhenProfileIsUpdated()
        {
            // Arrange
            var inputUser = new FoodOrderingWebServices.Models.User
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "password123",
                PhoneNumber = "1234567890",
                Address = "123 Street"
            };

            var returnedUser = new FoodOrderingDataAccessLayer.Models.User
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "password123",
                PhoneNumber = "1234567890",
                Address = "123 Street"
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.UpdateUserProfile(It.IsAny<FoodOrderingDataAccessLayer.Models.User>()))
                    .Returns(returnedUser);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.UpdateUserProfile(inputUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedUser = Assert.IsType<FoodOrderingDataAccessLayer.Models.User>(okResult.Value);
            Assert.Equal("john@example.com", updatedUser.Email);
        }

        [Fact]
        public void UpdateUserProfile_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var inputUser = new FoodOrderingWebServices.Models.User
            {
                FullName = "Jane Doe",
                Email = "jane@example.com",
                Password = "pass456",
                PhoneNumber = "9876543210",
                Address = "456 Avenue"
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.UpdateUserProfile(It.IsAny<FoodOrderingDataAccessLayer.Models.User>()))
                    .Returns((FoodOrderingDataAccessLayer.Models.User)null);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.UpdateUserProfile(inputUser);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User profile not found.", notFoundResult.Value);
        }

        [Fact]
        public void UpdateUserProfile_ReturnsServerError_OnException()
        {
            // Arrange
            var inputUser = new FoodOrderingWebServices.Models.User
            {
                FullName = "Error User",
                Email = "error@example.com",
                Password = "errorpass",
                PhoneNumber = "0000000000",
                Address = "Nowhere"
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.UpdateUserProfile(It.IsAny<FoodOrderingDataAccessLayer.Models.User>()))
                    .Throws(new Exception("DB Error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.UpdateUserProfile(inputUser);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("An error occurred while fetching user profile", errorResult.Value.ToString());
        }
        [Fact]
        public void DisplayRatingsByItem_ReturnsAverage_WhenAvailable()
        {
            // Arrange
            int menuItemId = 1;
            double expectedRating = 4.5;

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAverageRatingByMenuItemId(menuItemId))
                    .Returns(expectedRating);

            var service = new CustomerController(mockRepo.Object); // Replace with your actual class

            // Act
            var result = service.DisplayRatingsByItem(menuItemId);

            // Assert
            Assert.Equal(expectedRating, result);
        }

        [Fact]
        public void DisplayRatingsByItem_ReturnsZero_WhenRatingIsNull()
        {
            // Arrange
            int menuItemId = 2;

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAverageRatingByMenuItemId(menuItemId))
                    .Returns((double?)null);

            var service = new CustomerController(mockRepo.Object);

            // Act
            var result = service.DisplayRatingsByItem(menuItemId);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void DisplayRatingsByItem_ReturnsMinusOne_OnException()
        {
            // Arrange
            int menuItemId = 3;

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetAverageRatingByMenuItemId(menuItemId))
                    .Throws(new Exception("Database error"));

            var service = new CustomerController(mockRepo.Object);

            // Act
            var result = service.DisplayRatingsByItem(menuItemId);

            // Assert
            Assert.Equal(-1, result);
        }
        [Fact]
        public void AddRating_ReturnsBadRequest_WhenRatingIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IFoodRepository>();
            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddRating(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Rating details are required", badRequestResult.Value);
        }

        [Fact]
        public void AddRating_ReturnsOk_WhenAddSuccessful()
        {
            // Arrange
            var inputRating = new FoodOrderingWebServices.Models.Rating
            {
                MenuItemId = 1,
                RatingValue = 4,
                ItemName = "Pizza",
                Email = "user@example.com",
                OrderItemId = 100
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddRating(It.IsAny<FoodOrderingDataAccessLayer.Models.Rating>()))
                    .Returns(true);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddRating(inputRating);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Rating added successfully.", okResult.Value);
        }

        [Fact]
        public void AddRating_ReturnsBadRequest_WhenAddFails()
        {
            // Arrange
            var inputRating = new FoodOrderingWebServices.Models.Rating
            {
                MenuItemId = 1,
                RatingValue = 3,
                ItemName = "Burger",
                Email = "user2@example.com",
                OrderItemId = 101
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddRating(It.IsAny<FoodOrderingDataAccessLayer.Models.Rating>()))
                    .Returns(false);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddRating(inputRating);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to add rating.", badRequestResult.Value);
        }

        [Fact]
        public void AddRating_ReturnsServerError_OnException()
        {
            // Arrange
            var inputRating = new FoodOrderingWebServices.Models.Rating
            {
                MenuItemId = 1,
                RatingValue = 5,
                ItemName = "Pasta",
                Email = "user3@example.com",
                OrderItemId = 102
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.AddRating(It.IsAny<FoodOrderingDataAccessLayer.Models.Rating>()))
                    .Throws(new System.Exception("DB error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.AddRating(inputRating);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains("An error occurred while adding rating", objectResult.Value.ToString());
        }
        [Fact]
        public void GetRatingDetails_ReturnsNotFound_WhenNoRatings()
        {
            // Arrange
            var email = "test@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetRatingsByemail(email))
                    .Returns((List<FoodOrderingDataAccessLayer.Models.Rating>)null); // simulate null return

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetRatingDetails(email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No ratings found for the provided email.", notFoundResult.Value);
        }

        [Fact]
        public void GetRatingDetails_ReturnsNotFound_WhenEmptyRatings()
        {
            // Arrange
            var email = "empty@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetRatingsByemail(email))
                    .Returns(new List<FoodOrderingDataAccessLayer.Models.Rating>()); // empty list

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetRatingDetails(email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No ratings found for the provided email.", notFoundResult.Value);
        }

        [Fact]
        public void GetRatingDetails_ReturnsOk_WithRatings()
        {
            // Arrange
            var email = "user@example.com";
            var mockRatings = new List<FoodOrderingDataAccessLayer.Models.Rating>
            {
                new FoodOrderingDataAccessLayer.Models.Rating { MenuItemId = 1, RatingValue = 4, Email = email },
                new FoodOrderingDataAccessLayer.Models.Rating { MenuItemId = 2, RatingValue = 5, Email = email }
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetRatingsByemail(email))
                    .Returns(mockRatings);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetRatingDetails(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRatings = Assert.IsAssignableFrom<List<FoodOrderingDataAccessLayer.Models.Rating>>(okResult.Value);
            Assert.Equal(2, returnedRatings.Count);
        }

        [Fact]
        public void GetRatingDetails_ReturnsServerError_OnException()
        {
            // Arrange
            var email = "error@example.com";
            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetRatingsByemail(email))
                    .Throws(new System.Exception("Database error"));

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.GetRatingDetails(email);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains("An error occurred while fetching ratings", objectResult.Value.ToString());
        }

        [Fact]
        public void FilterMenuItems_ReturnsOk_WithFilteredItems()
        {
            // Arrange
            var filter = new FoodOrderingWebServices.Models.MenuFilter
            {
                CuisineIds = new List<int> { 1, 2 },
                IsVegetarian = true,
                MinPrice = 10,
                MaxPrice = 50
            };

            var filteredMenuItems = new List<FoodOrderingDataAccessLayer.Models.MenuItem>
            {
                new MenuItem { MenuItemId = 1, ItemName = "Veg Pizza", Price = 30 },
                new MenuItem { MenuItemId = 2, ItemName = "Veg Burger", Price = 20 }
            };

            var mockRepo = new Mock<IFoodRepository>();
            mockRepo.Setup(repo => repo.GetFilteredMenuItems(It.IsAny<FoodOrderingDataAccessLayer.Models.MenuFilter>()))
                    .Returns(filteredMenuItems);

            var controller = new CustomerController(mockRepo.Object);

            // Act
            var result = controller.FilterMenuItems(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItems = Assert.IsAssignableFrom<List<FoodOrderingDataAccessLayer.Models.MenuItem>>(okResult.Value);
            Assert.Equal(2, returnItems.Count);
        }

        [Fact]
        public void AddBooking_ValidBooking_ReturnsOk()
        {
            var booking = new FoodOrderingWebServices.Models.Booking
            {
                BookingId = 1,
                //BookingDate = DateTime.Today,
                //BookingTime = TimeSpan.FromHours(18),
                Email = "test@example.com",
                Guests = 2
            };

            _mockRepository.Setup(r => r.AddBooking(It.IsAny<FoodOrderingDataAccessLayer.Models.Booking>()))
                    .Returns(true);

            var result = _controller.AddBooking(booking) as OkObjectResult;

            Assert.NotNull(result);
            var returned = Assert.IsType<Booking>(result.Value);
            Assert.Equal(booking.Email, returned.Email);
        }

        [Fact]
        public void ViewBooking_WhenBookingsExist_ReturnsList()
        {
            var bookings = new List<FoodOrderingDataAccessLayer.Models.Booking>
            {
                new FoodOrderingDataAccessLayer.Models.Booking { BookingId = 1, Email = "test1@example.com" },
                new FoodOrderingDataAccessLayer.Models.Booking { BookingId = 2, Email = "test2@example.com" }
            };

            _mockRepository.Setup(r => r.ViewBookings()).Returns(bookings);

            var result = _controller.ViewBooking() as OkObjectResult;

            Assert.NotNull(result);
            var returned = Assert.IsType<List<FoodOrderingDataAccessLayer.Models.Booking>>(result.Value);
            Assert.Equal(2, returned.Count);
        }

        [Fact]
        public void ViewBooking_WhenNoBookings_ReturnsNotFound()
        {
            _mockRepository.Setup(r => r.ViewBookings()).Returns(new List<FoodOrderingDataAccessLayer.Models.Booking>());

            var result = _controller.ViewBooking();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GenerateBookingQR_ReturnsBase64Image()
        {
            var result = _controller.GenerateBookingQR() as OkObjectResult;

            Assert.NotNull(result);
            var value = result.Value.GetType().GetProperty("qrImage")?.GetValue(result.Value);
            Assert.NotNull(value);
            Assert.StartsWith("data:image", value.ToString());
        }

        [Fact]
        public void CheckIn_ValidId_ReturnsSuccess()
        {
            _mockRepository.Setup(r => r.CheckIn(1)).Returns(true);

            var result = _controller.CheckIn(1) as OkObjectResult;

            Assert.NotNull(result);
            var msg = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value)?.ToString();
            Assert.Equal("Checked in successfully.", msg);
        }

        [Fact]
        public void CheckIn_Failure_ReturnsServerError()
        {
            _mockRepository.Setup(r => r.CheckIn(99)).Returns(false);

            var result = _controller.CheckIn(99);

            Assert.IsType<ObjectResult>(result);
            var obj = result as ObjectResult;
            Assert.Equal(500, obj.StatusCode);
        }
    }

}
