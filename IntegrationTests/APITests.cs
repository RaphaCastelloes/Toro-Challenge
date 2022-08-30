using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace IntegrationTests
{
    public class APITests
    {
        // Test the authentication method
        [Fact]
        public void Authenticate()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            var client = new HttpClient();
            client.BaseAddress = new Uri(configuration["ApiUrl"]);
            var response = client.PostAsync("/api/Token/authenticate", new StringContent("{\"userName\":\"12345678901\",\"password\":\"password\"}", Encoding.UTF8, "application/json")).Result;
            Assert.True(response.IsSuccessStatusCode);
        }

        // Test the authentication state method when failed
        [Fact]
        public void Authenticate_Fail()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            var client = new HttpClient();
            client.BaseAddress = new Uri(configuration["ApiUrl"]);
            var response = client.PostAsync("/api/Token/authenticate", new StringContent("{\"userName\":\"12345678901\",\"password\":\"password1\"}", Encoding.UTF8, "application/json")).Result;
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        // Test the authentication state method when success
        [Fact]
        public void GetAuthState()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            var client = new HttpClient();
            client.BaseAddress = new Uri(configuration["ApiUrl"]);
            var response = client.PostAsync("/api/Token/authenticate", new StringContent("{\"userName\":\"12345678901\",\"password\":\"password\"}", Encoding.UTF8, "application/json")).Result;
            Assert.True(response.IsSuccessStatusCode);
            var token = response.Content.ReadAsStringAsync().Result;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            response = client.GetAsync("/api/Token/GetAuthenticationState").Result;
            Assert.True(response.IsSuccessStatusCode && response.Content.ReadAsStringAsync().Result == "Authenticated");
        }
    }
}
