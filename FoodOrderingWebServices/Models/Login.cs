using System.ComponentModel.DataAnnotations;

namespace FoodOrderingWebServices.Models
{
    public class Login
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
