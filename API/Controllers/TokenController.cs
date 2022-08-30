using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _IJwtTokenManager;

        public TokenController(IJwtTokenManager IJwtTokenManager)
        {
            _IJwtTokenManager = IJwtTokenManager;
        }

        // Authentication method
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserCredential userCredential)
        {
            var token = _IJwtTokenManager.Authenticate(userCredential.UserName, userCredential.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        // Returns the authentication state
        [HttpGet("GetAuthenticationState")]
        public IActionResult GetAuthenticationState()
        {
            return Ok("Authenticated");
        }
    }
}