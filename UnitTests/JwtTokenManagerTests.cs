using System;
using API.JWT;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTests
{
    public class JwtTokenManagerTests
    {
        // Test the authentication method
        [Fact]
        public void Authenticate()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var jwtTokenManager = new JwtTokenManager(configuration);
            var token = jwtTokenManager.Authenticate("12345678901", "password");
            Assert.NotNull(token);
        }

        // Test the authentication method when failed
        [Fact]
        public void Authenticate_Fail()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var jwtTokenManager = new JwtTokenManager(configuration);
            var token = jwtTokenManager.Authenticate("12345678901", "wrongpassword");
            Assert.Null(token);
        }
    }
}
