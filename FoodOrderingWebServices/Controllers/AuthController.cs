using FoodOrderingDataAccessLayer;
using FoodOrderingWebServices.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using UserService.Models;
using FoodOrderingWebServices.Models;
using BCrypt.Net;
namespace FoodOrderingWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private ITokenService _token;
        private IFoodRepository _foodRepo;
        private readonly UserService.Services.IEmailService _emailService;

        public AuthController(IFoodRepository repo, UserService.Services.IEmailService emailService, ITokenService tokenService)
        {
            _foodRepo = repo;
            _emailService = emailService;
            _token = tokenService;
        }

        [HttpPost] // Login
        public IActionResult Login([FromBody] Login model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Invalid login request");

            var user = _foodRepo.GetUserByMail(model.Email); // only pass email

            if (user == null)
                return Unauthorized("Invalid credentials");

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!isValidPassword)
                return Unauthorized("Invalid credentials");

            var token = _token.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                RoleId = user.RoleId,
                Email = user.Email,
                FullName = user.FullName
            });
        }

        [HttpPost] // Validation
        public JsonResult ValidateUserCredentials(Models.User userObj)
        {
            string roleName = "";
            try
            {
                var dal = new FoodRepository();
                roleName = dal.ValidateLogin(userObj.Email, userObj.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                roleName = "Invalid credentials";
            }

            return Json(roleName);
        }

        [HttpPost] // Register
        public IActionResult Register(Models.User newCustomer)
        {
            try
            {
                if (newCustomer == null)
                    return BadRequest("Customer details are Required");

                var customerToRegister = new FoodOrderingDataAccessLayer.Models.User
                {
                    FullName = newCustomer.FullName,
                    Email = newCustomer.Email,
                    Password = newCustomer.Password,
                    PhoneNumber = newCustomer.PhoneNumber,
                    Gender = newCustomer.Gender,
                    RoleId = 2 // Customer role  
                };

                var existingCustomer = _foodRepo.GetUserByMail(newCustomer.Email);
                if (existingCustomer != null)
                    return Conflict("Customer already exists");
                bool isRegistered = _foodRepo.RegisterCustomer(customerToRegister);
                if (isRegistered)
                {
                    return Ok(new { Message = "Registration successful" });
                }
                else
                {
                    return StatusCode(500, "Registration failed. Please try again");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProfile")] // Profile
        public IActionResult GetUserByMail(string mail)
        {
            var user = _foodRepo.GetUserByMail(mail);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("User not found.");
            }
        }

        [HttpGet] // Email Notification
        public IActionResult TestEmail(string email, string status)
        {
            try
            {
                var message = new Message(
                    new string[] { email },
                     "Issue Status has been Resolved " +
                     "Thanks & Regards from Supporting team!!",
                    "Your Issue Status"

                );

                _emailService.SendEmail(message);

                return Ok("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Email sending failed: {ex.Message}");
            }
        }

        [HttpGet] // OTP generation
        public IActionResult TestEmail2(string email, string code)
        {
            try
            {
                var body = $"Your verification code is: {code}";
                var message = new Message(
                    new string[] { email },
                    body,
                    "Your Code Sent"
                    
                );

                _emailService.SendEmail(message);

                return Ok("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Email sending failed: {ex.Message}");
            }
        }
    }
}
