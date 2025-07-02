using FoodOrderingDataAccessLayer.Models;

namespace FoodOrderingWebServices.Services
{
    public interface ITokenService
    {
        string GenerateToken(User users);
    }
}
