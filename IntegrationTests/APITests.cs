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

        // Test the authentication state method with expired token
        [Fact]
        public void GetAuthState_ExpiredToken()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            var client = new HttpClient();
            client.BaseAddress = new Uri(configuration["ApiUrl"]);
            // Expired token
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjYXN0ZWxsb2VzamZAaG90bWFpbC5jb20iLCJuYmYiOjE2NjE4NzM1NTcsImV4cCI6MTY2MTg3NTM1NywiaWF0IjoxNjYxODczNTU3fQ.trtEBBCp2j21nCwtJqZGqKdbpVG3GifLmmQcuI_8VHo";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = client.GetAsync("/api/Token/GetAuthenticationState").Result;
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
