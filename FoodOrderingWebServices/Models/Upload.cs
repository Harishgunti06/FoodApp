using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingWebServices.Models
{
    public class Upload
    {
        [FromForm]
        public IFormFile File { get; set; }
    }
}
