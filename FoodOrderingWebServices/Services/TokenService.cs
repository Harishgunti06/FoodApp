using FoodOrderingDataAccessLayer.Models;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodOrderingWebServices.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _iconfig;

        public TokenService(IConfiguration configuration)
        {
            _iconfig = configuration;
        }
        public string GenerateToken(User users)
        {
            var claims = new[]
            {
             new Claim(ClaimTypes.Name, users.Email),
               new Claim(ClaimTypes.Role, users.RoleId.ToString())
           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfig["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_iconfig["Jwt:ExpiresInMinutes"]));
            var token = new JwtSecurityToken(
                issuer: _iconfig["Jwt:Issuer"],
                audience: _iconfig["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
