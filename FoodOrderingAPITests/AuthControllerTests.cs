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
using FoodOrderingWebServices.Controllers;
using FoodOrderingWebServices.Services;
using UserService.Services;
using UserService.Models;

namespace FoodOrderingAPITests
{
    public class AuthControllerTests
    {
        private Mock<IFoodRepository> mockRepository;
        private Mock<IEmailService> mockEmailService;
        private Mock<ITokenService> mockTokenService;
        private AuthController controller;

        public AuthControllerTests()
        {
            mockRepository = new Mock<IFoodRepository>();
            mockEmailService = new Mock<IEmailService>();
            mockTokenService = new Mock<ITokenService>();
            controller = new AuthController(mockRepository.Object, mockEmailService.Object, mockTokenService.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsTokenAndUserInfo()
        {
            var loginModel = new FoodOrderingWebServices.Models.Login { Email = "test@example.com", Password = "123456" };
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginModel.Password);

            var dbUser = new User
            {
                Email = loginModel.Email,
                Password = hashedPassword,
                FullName = "Test User",
                RoleId = 2
            };

            mockRepository.Setup(r => r.GetUserByMail(loginModel.Email)).Returns(dbUser);
            mockTokenService.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("mocked_token");

            var result = controller.Login(loginModel) as OkObjectResult;

            Assert.NotNull(result);
            dynamic response = result.Value;
            Assert.Equal("mocked_token", response.Token);
            Assert.Equal("test@example.com", response.Email);
            Assert.Equal("Test User", response.FullName);
            Assert.Equal(2, response.RoleId);
        }

        [Fact]
        public void Register_NewUser_ReturnsSuccess()
        {
            var newUser = new FoodOrderingWebServices.Models.User
            {
                FullName = "New User",
                Email = "newuser@example.com",
                Password = "password123",
                PhoneNumber = "1234567890",
                Gender = "Female"
            };

            mockRepository.Setup(r => r.GetUserByMail(newUser.Email)).Returns((User)null);
            mockRepository.Setup(r => r.RegisterCustomer(It.IsAny<User>())).Returns(true);

            var result = controller.Register(newUser) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Registration successful", ((dynamic)result.Value).Message);
        }

        [Fact]
        public void GetUserByMail_ExistingEmail_ReturnsUser()
        {
            var email = "user@example.com";
            var user = new User { Email = email, FullName = "John Doe" };

            mockRepository.Setup(r => r.GetUserByMail(email)).Returns(user);

            var result = controller.GetUserByMail(email) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(user, result.Value);
        }

        [Fact]
        public void TestEmail_ValidRequest_ReturnsSuccess()
        {
            var email = "test@example.com";

            mockEmailService.Setup(e => e.SendEmail(It.IsAny<Message>()));

            var result = controller.TestEmail(email, "Resolved") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Test email sent successfully.", result.Value);
        }

        [Fact]
        public void TestEmail2_ValidRequest_ReturnsSuccess()
        {
            var email = "test@example.com";
            var code = "789456";

            mockEmailService.Setup(e => e.SendEmail(It.IsAny<Message>()));

            var result = controller.TestEmail2(email, code) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Test email sent successfully.", result.Value);
        }
    }
}
