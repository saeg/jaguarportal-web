namespace WebJaguarPortal.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using WebJaguarPortal.Infrastructure;
    using WebJaguarPortal.Models;
    using WebJaguarPortal.Repository.Interfaces;

    public class TokenService
    {
        private readonly IRepository<Settings> repository;

        public TokenService(IRepository<Settings> repository)
        {
            this.repository = repository;
        }

        public string GenerateToken(User user, IEnumerable<Claim> claims)
        {          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(repository.GetAll().First().JWTSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }

}
