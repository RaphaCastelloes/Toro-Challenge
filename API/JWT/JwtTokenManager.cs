using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Data;

namespace API.JWT
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration _configuration;

        public JwtTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Authentication method    
        public string Authenticate(string userName, string password)
        {
            // Validate user credential
            if (!IsValidCredential(userName, password))
            {
                return null;
            }

            // JwtConfiguration key in appsettings.json or appsettings.Development.json
            var key = _configuration["JwtConfig:Key"];

            // Convert key to byte array
            var keyBytes = Encoding.ASCII.GetBytes(key);

            // Instantiante JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                // Which user this claims belong to
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userName)
                }),
                // Define the token expiration time
                Expires = DateTime.UtcNow.AddMinutes(30),
                // Define the token signing credentials
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // Generate token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return token
            return tokenHandler.WriteToken(token);
        }

        // Validate if there is this credential in user source
        private bool IsValidCredential(string userName, string password)
        {
            // Check if userName not contains @
            if (!userName.Contains("@"))
            {
                userName = userName.Replace(".", "").Replace("-", "").Replace("/", "").Replace("\\", "");
            }

            return DataSource.Users.Any(u => u.Key == userName && u.Value == password);
        }
    }
}